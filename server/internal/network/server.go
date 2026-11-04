package network

import (
	"log"
	"shared/gen/go/messages"
	"time"

	"server/internal/game"

	"capnproto.org/go/capnp/v3"
	"github.com/codecat/go-enet"
)

const (
	channelReliable   uint8 = 0 // session, chat, events
	channelUnreliable uint8 = 1 // transforms
	channelCount            = 2

	flagsReliable   = enet.PacketFlagReliable
	flagsUnreliable = enet.PacketFlags(0)
)

type Config struct {
	Port       uint16
	MaxPlayers int
	SendRateHz uint16 // told to clients in Welcome
	ModVersion string // required client mod version; empty accepts any
	Verbose    bool   // log every transform
}

// Server owns the ENet host. ENet is not thread-safe, so every ENet call
// happens on the ListenLoop goroutine; peers is only touched there too.
type Server struct {
	cfg    Config
	host   enet.Host
	engine *game.Engine

	peers        map[enet.Peer]uint32 // connected peers -> player ID (0 until Hello is accepted)
	nextPacketID uint32

	stop chan struct{}
	done chan struct{}
}

func NewServer(cfg Config, engine *game.Engine) (*Server, error) {
	enet.Initialize()

	// A few spare ENet slots so clients over the limit get a proper "server full"
	// Reject instead of a silent connection timeout. ENet caps peers at 4095.
	peerSlots := min(cfg.MaxPlayers+4, 4095)

	host, err := enet.NewHost(enet.NewListenAddress(cfg.Port), uint64(peerSlots), channelCount, 0, 0)
	if err != nil {
		enet.Deinitialize()
		return nil, err
	}

	log.Printf("[network] enet server listening on udp port %d (max %d players)", cfg.Port, cfg.MaxPlayers)

	return &Server{
		cfg:    cfg,
		host:   host,
		engine: engine,
		peers:  make(map[enet.Peer]uint32),
		stop:   make(chan struct{}),
		done:   make(chan struct{}),
	}, nil
}

func (s *Server) ListenLoop() {
	defer close(s.done)
	log.Println("[network] enet listening loop started.")

	for {
		select {
		case <-s.stop:
			s.disconnectAll()
			return
		default:
		}

		// Wait up to 15ms for an event.
		ev := s.host.Service(15)

		switch ev.GetType() {
		case enet.EventConnect:
			peer := ev.GetPeer()
			s.peers[peer] = 0
			log.Printf("[network] peer connected from %s, waiting for hello", peer.GetAddress())

		case enet.EventDisconnect:
			s.handleDisconnect(ev.GetPeer())

		case enet.EventReceive:
			s.handlePacket(ev.GetPeer(), ev.GetPacket())
		}
	}
}

func (s *Server) Shutdown() {
	log.Println("[network] tearing down enet host...")
	close(s.stop)
	<-s.done // wait for ListenLoop to exit before freeing the host it uses
	s.host.Destroy()
	enet.Deinitialize()
}

func (s *Server) disconnectAll() {
	for peer := range s.peers {
		peer.DisconnectNow(0)
	}
	s.peers = map[enet.Peer]uint32{}
}

func (s *Server) handleDisconnect(peer enet.Peer) {
	id, ok := s.peers[peer]
	delete(s.peers, peer)
	if !ok || id == 0 {
		log.Printf("[network] peer %s disconnected before joining", peer.GetAddress())
		return
	}

	p, ok := s.engine.RemovePlayer(id)
	if !ok {
		return
	}
	log.Printf("[network] player %d (%s) left", p.ID, p.Name)

	s.broadcast(allPlayers, channelReliable, flagsReliable, func(b messages.NetworkPacket_body) error {
		b.SetPlayerLeft(id)
		return nil
	})
}

// ---------- sending ----------

type bodyFn func(body messages.NetworkPacket_body) error

func (s *Server) encode(fill bodyFn) ([]byte, error) {
	msg, seg, err := capnp.NewMessage(capnp.SingleSegment(nil))
	if err != nil {
		return nil, err
	}
	pkt, err := messages.NewRootNetworkPacket(seg)
	if err != nil {
		return nil, err
	}
	s.nextPacketID++
	pkt.SetId(s.nextPacketID)
	pkt.SetTimestamp(time.Now().UnixMilli())
	if err := fill(pkt.Body()); err != nil {
		return nil, err
	}
	return msg.Marshal() // unpacked: the C# runtime can't read packed messages
}

func (s *Server) send(peer enet.Peer, channel uint8, flags enet.PacketFlags, fill bodyFn) {
	data, err := s.encode(fill)
	if err != nil {
		log.Printf("[network] failed to encode packet: %v", err)
		return
	}
	if err := peer.SendBytes(data, channel, flags); err != nil {
		log.Printf("[network] send to %s failed: %v", peer.GetAddress(), err)
	}
}

// broadcast encodes once and sends to every joined player that passes filter.
func (s *Server) broadcast(filter func(game.Player) bool, channel uint8, flags enet.PacketFlags, fill bodyFn) {
	data, err := s.encode(fill)
	if err != nil {
		log.Printf("[network] failed to encode broadcast: %v", err)
		return
	}
	for peer, id := range s.peers {
		if id == 0 {
			continue
		}
		p, ok := s.engine.Player(id)
		if !ok || !filter(p) {
			continue
		}
		if err := peer.SendBytes(data, channel, flags); err != nil {
			log.Printf("[network] broadcast to player %d failed: %v", id, err)
		}
	}
}

// Broadcast filters.

func allPlayers(game.Player) bool { return true }

func allExcept(id uint32) func(game.Player) bool {
	return func(p game.Player) bool { return p.ID != id }
}

func sameZoneExcept(id uint32, zone string) func(game.Player) bool {
	return func(p game.Player) bool { return p.ID != id && p.Zone == zone }
}
