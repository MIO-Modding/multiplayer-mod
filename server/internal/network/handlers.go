package network

import (
	"fmt"
	"log"
	"shared/gen/go/messages"
	"strings"

	"server/internal/game"

	"capnproto.org/go/capnp/v3"
	"github.com/codecat/go-enet"
)

const (
	maxNameLen = 32
	maxChatLen = 256
	maxZoneLen = 128
)

func (s *Server) handlePacket(peer enet.Peer, packet enet.Packet) {
	defer packet.Destroy()

	msg, err := capnp.Unmarshal(packet.GetData())
	if err != nil {
		log.Printf("[network] corrupt packet from %s dropped: %v", peer.GetAddress(), err)
		return
	}
	pkt, err := messages.ReadRootNetworkPacket(msg)
	if err != nil {
		log.Printf("[network] failed to parse packet from %s: %v", peer.GetAddress(), err)
		return
	}
	body := pkt.Body()

	if body.Which() == messages.NetworkPacket_body_Which_hello {
		s.handleHello(peer, body)
		return
	}

	id := s.peers[peer]
	if id == 0 {
		log.Printf("[network] %s sent %v before hello, ignoring", peer.GetAddress(), body.Which())
		return
	}

	switch body.Which() {
	case messages.NetworkPacket_body_Which_ping:
		s.send(peer, channelReliable, flagsReliable, func(b messages.NetworkPacket_body) error {
			b.SetPong()
			return nil
		})
	case messages.NetworkPacket_body_Which_pong:
		// nothing to do
	case messages.NetworkPacket_body_Which_playerTransform:
		s.handleTransform(id, body)
	case messages.NetworkPacket_body_Which_chatMessage:
		s.handleChat(id, body)
	case messages.NetworkPacket_body_Which_zoneChange:
		s.handleZoneChange(id, body)
	case messages.NetworkPacket_body_Which_playerEvent:
		s.handlePlayerEvent(id, body)
	default:
		log.Printf("[network] player %d sent server-only packet %v, ignoring", id, body.Which())
	}
}

// ---------- session ----------

func (s *Server) handleHello(peer enet.Peer, body messages.NetworkPacket_body) {
	if s.peers[peer] != 0 {
		return // already joined
	}

	h, err := body.Hello()
	if err != nil {
		return
	}
	name, _ := h.PlayerName()
	modVersion, _ := h.ModVersion()

	switch {
	case h.ProtocolVersion() != messages.CurrentProtocolVersion:
		s.reject(peer, messages.Reject_RejectReason_versionMismatch,
			fmt.Sprintf("protocol version %d, server needs %d", h.ProtocolVersion(), messages.CurrentProtocolVersion))
		return
	case s.cfg.ModVersion != "" && modVersion != s.cfg.ModVersion:
		s.reject(peer, messages.Reject_RejectReason_versionMismatch,
			fmt.Sprintf("mod version %q, server needs %q", modVersion, s.cfg.ModVersion))
		return
	case s.engine.Count() >= s.cfg.MaxPlayers:
		s.reject(peer, messages.Reject_RejectReason_serverFull, "server is full")
		return
	}

	name = clean(name, maxNameLen)
	if name == "" {
		name = "player"
	}

	p := s.engine.AddPlayer(name)
	s.peers[peer] = p.ID
	log.Printf("[network] player %d (%s) joined from %s", p.ID, p.Name, peer.GetAddress())

	s.send(peer, channelReliable, flagsReliable, func(b messages.NetworkPacket_body) error {
		w, err := b.NewWelcome()
		if err != nil {
			return err
		}
		w.SetPlayerId(p.ID)
		w.SetSendRateHz(s.cfg.SendRateHz)
		return nil
	})

	s.sendSnapshot(peer, p.ID)

	s.broadcast(allExcept(p.ID), channelReliable, flagsReliable, func(b messages.NetworkPacket_body) error {
		info, err := b.NewPlayerJoined()
		if err != nil {
			return err
		}
		return fillPlayerInfo(info, p)
	})
}

func (s *Server) sendSnapshot(peer enet.Peer, selfID uint32) {
	var others []game.Player
	for _, p := range s.engine.Players() {
		if p.ID != selfID {
			others = append(others, p)
		}
	}

	s.send(peer, channelReliable, flagsReliable, func(b messages.NetworkPacket_body) error {
		snap, err := b.NewSnapshot()
		if err != nil {
			return err
		}
		list, err := snap.NewPlayers(int32(len(others)))
		if err != nil {
			return err
		}
		for i, p := range others {
			ps := list.At(i)
			info, err := ps.NewInfo()
			if err != nil {
				return err
			}
			if err := fillPlayerInfo(info, p); err != nil {
				return err
			}
			t, err := ps.NewTransform()
			if err != nil {
				return err
			}
			fillTransform(t, p)
		}
		return nil
	})
}

func (s *Server) reject(peer enet.Peer, reason messages.Reject_RejectReason, message string) {
	log.Printf("[network] rejecting %s: %s", peer.GetAddress(), message)
	s.send(peer, channelReliable, flagsReliable, func(b messages.NetworkPacket_body) error {
		r, err := b.NewReject()
		if err != nil {
			return err
		}
		r.SetReason(reason)
		return r.SetDetail(message)
	})
	peer.DisconnectLater(0) // disconnects after the reject is delivered
}

// ---------- movement / world ----------

func (s *Server) handleTransform(id uint32, body messages.NetworkPacket_body) {
	t, err := body.PlayerTransform()
	if err != nil {
		return
	}

	p, ok := s.engine.UpdateTransform(id, game.Transform{
		X: t.X(), Y: t.Y(), Z: t.Z(), Yaw: t.Yaw(),
		VX: t.Vx(), VY: t.Vy(), VZ: t.Vz(),
		Anim: uint16(t.Anim()),
	})
	if !ok {
		return
	}

	if s.cfg.Verbose {
		log.Printf("[game] player %d @ (%.2f, %.2f, %.2f) yaw %.2f zone %q",
			id, p.Transform.X, p.Transform.Y, p.Transform.Z, p.Transform.Yaw, p.Zone)
	}

	s.broadcast(sameZoneExcept(id, p.Zone), channelUnreliable, flagsUnreliable, func(b messages.NetworkPacket_body) error {
		out, err := b.NewPlayerTransform()
		if err != nil {
			return err
		}
		fillTransform(out, p)
		return nil
	})
}

func (s *Server) handleZoneChange(id uint32, body messages.NetworkPacket_body) {
	z, err := body.ZoneChange()
	if err != nil {
		return
	}
	zone, _ := z.Zone()
	zone = clean(zone, maxZoneLen)

	if _, ok := s.engine.SetZone(id, zone); !ok {
		return
	}
	log.Printf("[game] player %d entered zone %q", id, zone)

	s.broadcast(allExcept(id), channelReliable, flagsReliable, func(b messages.NetworkPacket_body) error {
		out, err := b.NewZoneChange()
		if err != nil {
			return err
		}
		out.SetPlayerId(id)
		return out.SetZone(zone)
	})
}

// ---------- chat ----------

func (s *Server) handleChat(id uint32, body messages.NetworkPacket_body) {
	text, err := body.ChatMessage()
	if err != nil {
		return
	}
	text = clean(text, maxChatLen)
	if text == "" {
		return
	}

	p, _ := s.engine.Player(id)
	log.Printf("[chat] %s: %s", p.Name, text)

	// Sent to everyone, including the sender, so their client can show it.
	s.broadcast(allPlayers, channelReliable, flagsReliable, func(b messages.NetworkPacket_body) error {
		c, err := b.NewChatBroadcast()
		if err != nil {
			return err
		}
		c.SetPlayerId(id)
		return c.SetText(text)
	})
}

// ---------- gameplay ----------

func (s *Server) handlePlayerEvent(id uint32, body messages.NetworkPacket_body) {
	in, err := body.PlayerEvent()
	if err != nil {
		return
	}
	kind := in.Kind()

	p, ok := s.engine.Player(id)
	if !ok {
		return
	}

	s.broadcast(sameZoneExcept(id, p.Zone), channelReliable, flagsReliable, func(b messages.NetworkPacket_body) error {
		out, err := b.NewPlayerEvent()
		if err != nil {
			return err
		}
		out.SetPlayerId(id) // never trust the client's playerId
		switch kind.Which() {
		case messages.PlayerEvent_kind_Which_attack:
			out.Kind().SetAttack()
		case messages.PlayerEvent_kind_Which_hit:
			out.Kind().SetHit(kind.Hit())
		case messages.PlayerEvent_kind_Which_died:
			out.Kind().SetDied()
		case messages.PlayerEvent_kind_Which_respawned:
			out.Kind().SetRespawned()
		default:
			return fmt.Errorf("unknown player event kind %v", kind.Which())
		}
		return nil
	})
}

// ---------- helpers ----------

func fillPlayerInfo(info messages.PlayerInfo, p game.Player) error {
	info.SetPlayerId(p.ID)
	if err := info.SetName(p.Name); err != nil {
		return err
	}
	return info.SetZone(p.Zone)
}

func fillTransform(t messages.PlayerTransform, p game.Player) {
	t.SetPlayerId(p.ID)
	t.SetX(p.Transform.X)
	t.SetY(p.Transform.Y)
	t.SetZ(p.Transform.Z)
	t.SetYaw(p.Transform.Yaw)
	t.SetVx(p.Transform.VX)
	t.SetVy(p.Transform.VY)
	t.SetVz(p.Transform.VZ)
	t.SetAnim(messages.PlayerTransform_AnimState(p.Transform.Anim))
}

// clean trims whitespace and caps length (in runes) of client-supplied text.
func clean(s string, maxLen int) string {
	s = strings.TrimSpace(s)
	if r := []rune(s); len(r) > maxLen {
		s = string(r[:maxLen])
	}
	return s
}
