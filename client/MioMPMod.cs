using System;
using System.IO;
using System.Net;
using Capnp;
using CapnpGen;
using ENet.Managed;
using MioGame;
using MioModdingApi;
using MioModLoader;

namespace MioMP
{
    public unsafe class MioMPMod : Mod
    {
        const int ServerPort = 8095;
        const ushort ProtocolVersion = 1; // must match currentProtocolVersion in game.capnp
        const string ModVersion = "0.1.0";

        const byte ChannelReliable = 0;
        const byte ChannelUnreliable = 1;
        const byte ChannelCount = 2;

        static MioMPMod instance = null!;
        // Keep hook delegates alive so the GC doesn't collect them.
        static On.MioGame.On_Game.orig_update updateSuffix = null!;

        ENetHost host = null!;
        ENetPeer peer;
        uint playerId;          // 0 until the server sends Welcome
        double sendInterval = 1.0;
        double timeSinceSend;
        uint packetId;
        string currentZone = "";
        bool zoneDirty;         // resend currentZone to the server on the next tick

        public override void Initialize()
        {
            instance = this;

            ManagedENet.Startup();
            host = new ENetHost(null, 1, ChannelCount);
            peer = host.Connect(new IPEndPoint(IPAddress.Loopback, ServerPort), ChannelCount, 0);
            LogMessage($"[MioMP] connecting to 127.0.0.1:{ServerPort}");

            updateSuffix = OnGameUpdate;
            On.MioGame.On_Game.update.Suffix += updateSuffix;
        }

        // ---------- hooks (game thread) ----------

        static void OnGameUpdate(Game* game, float dtFrame) => instance.Tick(game, dtFrame);

        void Tick(Game* game, float dtFrame)
        {
            PumpNetwork();

            if (playerId == 0)
                return; // not joined yet

            string zone = Util.MioStringToString(game->current_zone_id);
            if (zone != currentZone)
            {
                currentZone = zone;
                zoneDirty = true;
            }

            if (zoneDirty)
            {
                zoneDirty = false;
                Send(ChannelReliable, ENetPacketFlags.Reliable, b => b.ZoneChange = new ZoneChange { Zone = currentZone });
                LogMessage($"[MioMP] zone -> {currentZone}");
            }

            // Only send movement once a save is loaded and Mio exists.
            if (Globals.metagame.state != Metagame.State.Game || game->mio.node == null)
                return;

            timeSinceSend += dtFrame;
            if (timeSinceSend < sendInterval)
                return;
            timeSinceSend = 0;

            Vec_float_3 pos = game->mio.node->_transform.translation;
            Vec_float_3 vel = game->mio.velocity;
            float yaw = game->mio.look_angle;

            Send(ChannelUnreliable, default, b => b.PlayerTransform = new PlayerTransform
            {
                PlayerId = playerId,
                X = pos.x,
                Y = pos.y,
                Z = pos.z,
                Yaw = yaw,
                Vx = vel.x,
                Vy = vel.y,
                Vz = vel.z,
            });
        }

        // ---------- network ----------

        void PumpNetwork()
        {
            while (true)
            {
                var ev = host.Service(TimeSpan.Zero); // non-blocking
                switch (ev.Type)
                {
                    case ENetEventType.None:
                        return;

                    case ENetEventType.Connect:
                        LogMessage("[MioMP] connected, sending hello");
                        Send(ChannelReliable, ENetPacketFlags.Reliable, b => b.Hello = new Hello
                        {
                            ProtocolVersion = ProtocolVersion,
                            ModVersion = ModVersion,
                            PlayerName = SteamName.Get(),
                        });
                        break;

                    case ENetEventType.Disconnect:
                        LogMessage("[MioMP] disconnected");
                        playerId = 0;
                        break;

                    case ENetEventType.Receive:
                        byte[] data = ev.Packet.Data.ToArray();
                        ev.Packet.Destroy();
                        HandlePacket(data);
                        break;
                }
            }
        }

        void HandlePacket(byte[] data)
        {
            var frame = Framing.ReadSegments(new MemoryStream(data));
            var packet = NetworkPacket.READER.create(DeserializerState.CreateRoot(frame));
            var body = packet.Body;

            switch (body.which)
            {
                case NetworkPacket.body.WHICH.Welcome:
                    playerId = body.Welcome.PlayerId;
                    if (body.Welcome.SendRateHz > 0)
                        sendInterval = 1.0 / body.Welcome.SendRateHz;
                    zoneDirty = true; // tell the server where we are
                    LogMessage($"[MioMP] joined as player {playerId}, sending at {body.Welcome.SendRateHz} Hz");
                    break;

                case NetworkPacket.body.WHICH.Reject:
                    LogMessage("[MioMP] rejected by server (see server log for why)");
                    break;

                case NetworkPacket.body.WHICH.PlayerJoined:
                    LogMessage($"[MioMP] {body.PlayerJoined.Name} (#{body.PlayerJoined.PlayerId}) joined");
                    break;

                case NetworkPacket.body.WHICH.PlayerLeft:
                    LogMessage($"[MioMP] player #{body.PlayerLeft} left");
                    break;

                case NetworkPacket.body.WHICH.ChatBroadcast:
                    LogMessage($"[MioMP] chat #{body.ChatBroadcast.PlayerId}: {body.ChatBroadcast.Text}");
                    break;

                    // playerTransform, zoneChange, snapshot, playerEvent: needed once we draw other players.
            }
        }

        void Send(byte channel, ENetPacketFlags flags, Action<NetworkPacket.body> fillBody)
        {
            var packet = new NetworkPacket
            {
                Id = packetId++,
                Timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                Body = new NetworkPacket.body(),
            };
            fillBody(packet.Body);

            var msg = MessageBuilder.Create();
            packet.serialize(msg.BuildRoot<NetworkPacket.WRITER>());

            // Standard unpacked capnp framing, matching capnp.Unmarshal on the Go side.
            using var ms = new MemoryStream();
            new FramePump(ms).Send(msg.Frame);

            peer.Send(channel, ms.ToArray(), flags);
        }
    }
}