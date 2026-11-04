using Capnp;
using Capnp.Rpc;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CapnpGen
{
    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xeeb80fbaecc40ea0UL)]
    public class NetworkPacket : ICapnpSerializable
    {
        public const UInt64 typeId = 0xeeb80fbaecc40ea0UL;
        void ICapnpSerializable.Deserialize(DeserializerState arg_)
        {
            var reader = READER.create(arg_);
            Id = reader.Id;
            Timestamp = reader.Timestamp;
            Body = CapnpSerializable.Create<CapnpGen.NetworkPacket.body>(reader.Body);
            applyDefaults();
        }

        public void serialize(WRITER writer)
        {
            writer.Id = Id;
            writer.Timestamp = Timestamp;
            Body?.serialize(writer.Body);
        }

        void ICapnpSerializable.Serialize(SerializerState arg_)
        {
            serialize(arg_.Rewrap<WRITER>());
        }

        public void applyDefaults()
        {
        }

        public uint Id
        {
            get;
            set;
        }

        public long Timestamp
        {
            get;
            set;
        }

        public CapnpGen.NetworkPacket.body Body
        {
            get;
            set;
        }

        public struct READER
        {
            readonly DeserializerState ctx;
            public READER(DeserializerState ctx)
            {
                this.ctx = ctx;
            }

            public static READER create(DeserializerState ctx) => new READER(ctx);
            public static implicit operator DeserializerState(READER reader) => reader.ctx;
            public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
            public uint Id => ctx.ReadDataUInt(0UL, 0U);
            public long Timestamp => ctx.ReadDataLong(64UL, 0L);
            public body.READER Body => new body.READER(ctx);
        }

        public class WRITER : SerializerState
        {
            public WRITER()
            {
                this.SetStruct(3, 1);
            }

            public uint Id
            {
                get => this.ReadDataUInt(0UL, 0U);
                set => this.WriteData(0UL, value, 0U);
            }

            public long Timestamp
            {
                get => this.ReadDataLong(64UL, 0L);
                set => this.WriteData(64UL, value, 0L);
            }

            public body.WRITER Body
            {
                get => Rewrap<body.WRITER>();
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xcdd41976af3035dfUL)]
        public class body : ICapnpSerializable
        {
            public const UInt64 typeId = 0xcdd41976af3035dfUL;
            public enum WHICH : ushort
            {
                Ping = 0,
                Pong = 1,
                PlayerTransform = 2,
                ChatMessage = 3,
                ChatBroadcast = 4,
                Hello = 5,
                Welcome = 6,
                Reject = 7,
                PlayerJoined = 8,
                PlayerLeft = 9,
                ZoneChange = 10,
                Snapshot = 11,
                PlayerEvent = 12,
                undefined = 65535
            }

            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                switch (reader.which)
                {
                    case WHICH.Ping:
                        which = reader.which;
                        break;
                    case WHICH.Pong:
                        which = reader.which;
                        break;
                    case WHICH.PlayerTransform:
                        PlayerTransform = CapnpSerializable.Create<CapnpGen.PlayerTransform>(reader.PlayerTransform);
                        break;
                    case WHICH.ChatMessage:
                        ChatMessage = reader.ChatMessage;
                        break;
                    case WHICH.ChatBroadcast:
                        ChatBroadcast = CapnpSerializable.Create<CapnpGen.ChatBroadcast>(reader.ChatBroadcast);
                        break;
                    case WHICH.Hello:
                        Hello = CapnpSerializable.Create<CapnpGen.Hello>(reader.Hello);
                        break;
                    case WHICH.Welcome:
                        Welcome = CapnpSerializable.Create<CapnpGen.Welcome>(reader.Welcome);
                        break;
                    case WHICH.Reject:
                        Reject = CapnpSerializable.Create<CapnpGen.Reject>(reader.Reject);
                        break;
                    case WHICH.PlayerJoined:
                        PlayerJoined = CapnpSerializable.Create<CapnpGen.PlayerInfo>(reader.PlayerJoined);
                        break;
                    case WHICH.PlayerLeft:
                        PlayerLeft = reader.PlayerLeft;
                        break;
                    case WHICH.ZoneChange:
                        ZoneChange = CapnpSerializable.Create<CapnpGen.ZoneChange>(reader.ZoneChange);
                        break;
                    case WHICH.Snapshot:
                        Snapshot = CapnpSerializable.Create<CapnpGen.Snapshot>(reader.Snapshot);
                        break;
                    case WHICH.PlayerEvent:
                        PlayerEvent = CapnpSerializable.Create<CapnpGen.PlayerEvent>(reader.PlayerEvent);
                        break;
                }

                applyDefaults();
            }

            private WHICH _which = WHICH.undefined;
            private object _content;
            public WHICH which
            {
                get => _which;
                set
                {
                    if (value == _which)
                        return;
                    _which = value;
                    switch (value)
                    {
                        case WHICH.Ping:
                            break;
                        case WHICH.Pong:
                            break;
                        case WHICH.PlayerTransform:
                            _content = null;
                            break;
                        case WHICH.ChatMessage:
                            _content = null;
                            break;
                        case WHICH.ChatBroadcast:
                            _content = null;
                            break;
                        case WHICH.Hello:
                            _content = null;
                            break;
                        case WHICH.Welcome:
                            _content = null;
                            break;
                        case WHICH.Reject:
                            _content = null;
                            break;
                        case WHICH.PlayerJoined:
                            _content = null;
                            break;
                        case WHICH.PlayerLeft:
                            _content = 0;
                            break;
                        case WHICH.ZoneChange:
                            _content = null;
                            break;
                        case WHICH.Snapshot:
                            _content = null;
                            break;
                        case WHICH.PlayerEvent:
                            _content = null;
                            break;
                    }
                }
            }

            public void serialize(WRITER writer)
            {
                writer.which = which;
                switch (which)
                {
                    case WHICH.Ping:
                        break;
                    case WHICH.Pong:
                        break;
                    case WHICH.PlayerTransform:
                        PlayerTransform?.serialize(writer.PlayerTransform);
                        break;
                    case WHICH.ChatMessage:
                        writer.ChatMessage = ChatMessage;
                        break;
                    case WHICH.ChatBroadcast:
                        ChatBroadcast?.serialize(writer.ChatBroadcast);
                        break;
                    case WHICH.Hello:
                        Hello?.serialize(writer.Hello);
                        break;
                    case WHICH.Welcome:
                        Welcome?.serialize(writer.Welcome);
                        break;
                    case WHICH.Reject:
                        Reject?.serialize(writer.Reject);
                        break;
                    case WHICH.PlayerJoined:
                        PlayerJoined?.serialize(writer.PlayerJoined);
                        break;
                    case WHICH.PlayerLeft:
                        writer.PlayerLeft = PlayerLeft.Value;
                        break;
                    case WHICH.ZoneChange:
                        ZoneChange?.serialize(writer.ZoneChange);
                        break;
                    case WHICH.Snapshot:
                        Snapshot?.serialize(writer.Snapshot);
                        break;
                    case WHICH.PlayerEvent:
                        PlayerEvent?.serialize(writer.PlayerEvent);
                        break;
                }
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public CapnpGen.PlayerTransform PlayerTransform
            {
                get => _which == WHICH.PlayerTransform ? (CapnpGen.PlayerTransform)_content : null;
                set
                {
                    _which = WHICH.PlayerTransform;
                    _content = value;
                }
            }

            public string ChatMessage
            {
                get => _which == WHICH.ChatMessage ? (string)_content : null;
                set
                {
                    _which = WHICH.ChatMessage;
                    _content = value;
                }
            }

            public CapnpGen.ChatBroadcast ChatBroadcast
            {
                get => _which == WHICH.ChatBroadcast ? (CapnpGen.ChatBroadcast)_content : null;
                set
                {
                    _which = WHICH.ChatBroadcast;
                    _content = value;
                }
            }

            public CapnpGen.Hello Hello
            {
                get => _which == WHICH.Hello ? (CapnpGen.Hello)_content : null;
                set
                {
                    _which = WHICH.Hello;
                    _content = value;
                }
            }

            public CapnpGen.Welcome Welcome
            {
                get => _which == WHICH.Welcome ? (CapnpGen.Welcome)_content : null;
                set
                {
                    _which = WHICH.Welcome;
                    _content = value;
                }
            }

            public CapnpGen.Reject Reject
            {
                get => _which == WHICH.Reject ? (CapnpGen.Reject)_content : null;
                set
                {
                    _which = WHICH.Reject;
                    _content = value;
                }
            }

            public CapnpGen.PlayerInfo PlayerJoined
            {
                get => _which == WHICH.PlayerJoined ? (CapnpGen.PlayerInfo)_content : null;
                set
                {
                    _which = WHICH.PlayerJoined;
                    _content = value;
                }
            }

            public uint? PlayerLeft
            {
                get => _which == WHICH.PlayerLeft ? (uint? )_content : null;
                set
                {
                    _which = WHICH.PlayerLeft;
                    _content = value;
                }
            }

            public CapnpGen.ZoneChange ZoneChange
            {
                get => _which == WHICH.ZoneChange ? (CapnpGen.ZoneChange)_content : null;
                set
                {
                    _which = WHICH.ZoneChange;
                    _content = value;
                }
            }

            public CapnpGen.Snapshot Snapshot
            {
                get => _which == WHICH.Snapshot ? (CapnpGen.Snapshot)_content : null;
                set
                {
                    _which = WHICH.Snapshot;
                    _content = value;
                }
            }

            public CapnpGen.PlayerEvent PlayerEvent
            {
                get => _which == WHICH.PlayerEvent ? (CapnpGen.PlayerEvent)_content : null;
                set
                {
                    _which = WHICH.PlayerEvent;
                    _content = value;
                }
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
                public WHICH which => (WHICH)ctx.ReadDataUShort(32U, (ushort)0);
                public CapnpGen.PlayerTransform.READER PlayerTransform => which == WHICH.PlayerTransform ? ctx.ReadStruct(0, CapnpGen.PlayerTransform.READER.create) : default;
                public string ChatMessage => which == WHICH.ChatMessage ? ctx.ReadText(0, null) : default;
                public CapnpGen.ChatBroadcast.READER ChatBroadcast => which == WHICH.ChatBroadcast ? ctx.ReadStruct(0, CapnpGen.ChatBroadcast.READER.create) : default;
                public CapnpGen.Hello.READER Hello => which == WHICH.Hello ? ctx.ReadStruct(0, CapnpGen.Hello.READER.create) : default;
                public CapnpGen.Welcome.READER Welcome => which == WHICH.Welcome ? ctx.ReadStruct(0, CapnpGen.Welcome.READER.create) : default;
                public CapnpGen.Reject.READER Reject => which == WHICH.Reject ? ctx.ReadStruct(0, CapnpGen.Reject.READER.create) : default;
                public CapnpGen.PlayerInfo.READER PlayerJoined => which == WHICH.PlayerJoined ? ctx.ReadStruct(0, CapnpGen.PlayerInfo.READER.create) : default;
                public uint PlayerLeft => which == WHICH.PlayerLeft ? ctx.ReadDataUInt(128UL, 0U) : default;
                public CapnpGen.ZoneChange.READER ZoneChange => which == WHICH.ZoneChange ? ctx.ReadStruct(0, CapnpGen.ZoneChange.READER.create) : default;
                public CapnpGen.Snapshot.READER Snapshot => which == WHICH.Snapshot ? ctx.ReadStruct(0, CapnpGen.Snapshot.READER.create) : default;
                public CapnpGen.PlayerEvent.READER PlayerEvent => which == WHICH.PlayerEvent ? ctx.ReadStruct(0, CapnpGen.PlayerEvent.READER.create) : default;
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                }

                public WHICH which
                {
                    get => (WHICH)this.ReadDataUShort(32U, (ushort)0);
                    set => this.WriteData(32U, (ushort)value, (ushort)0);
                }

                public CapnpGen.PlayerTransform.WRITER PlayerTransform
                {
                    get => which == WHICH.PlayerTransform ? BuildPointer<CapnpGen.PlayerTransform.WRITER>(0) : default;
                    set => Link(0, value);
                }

                public string ChatMessage
                {
                    get => which == WHICH.ChatMessage ? this.ReadText(0, null) : default;
                    set => this.WriteText(0, value, null);
                }

                public CapnpGen.ChatBroadcast.WRITER ChatBroadcast
                {
                    get => which == WHICH.ChatBroadcast ? BuildPointer<CapnpGen.ChatBroadcast.WRITER>(0) : default;
                    set => Link(0, value);
                }

                public CapnpGen.Hello.WRITER Hello
                {
                    get => which == WHICH.Hello ? BuildPointer<CapnpGen.Hello.WRITER>(0) : default;
                    set => Link(0, value);
                }

                public CapnpGen.Welcome.WRITER Welcome
                {
                    get => which == WHICH.Welcome ? BuildPointer<CapnpGen.Welcome.WRITER>(0) : default;
                    set => Link(0, value);
                }

                public CapnpGen.Reject.WRITER Reject
                {
                    get => which == WHICH.Reject ? BuildPointer<CapnpGen.Reject.WRITER>(0) : default;
                    set => Link(0, value);
                }

                public CapnpGen.PlayerInfo.WRITER PlayerJoined
                {
                    get => which == WHICH.PlayerJoined ? BuildPointer<CapnpGen.PlayerInfo.WRITER>(0) : default;
                    set => Link(0, value);
                }

                public uint PlayerLeft
                {
                    get => which == WHICH.PlayerLeft ? this.ReadDataUInt(128UL, 0U) : default;
                    set => this.WriteData(128UL, value, 0U);
                }

                public CapnpGen.ZoneChange.WRITER ZoneChange
                {
                    get => which == WHICH.ZoneChange ? BuildPointer<CapnpGen.ZoneChange.WRITER>(0) : default;
                    set => Link(0, value);
                }

                public CapnpGen.Snapshot.WRITER Snapshot
                {
                    get => which == WHICH.Snapshot ? BuildPointer<CapnpGen.Snapshot.WRITER>(0) : default;
                    set => Link(0, value);
                }

                public CapnpGen.PlayerEvent.WRITER PlayerEvent
                {
                    get => which == WHICH.PlayerEvent ? BuildPointer<CapnpGen.PlayerEvent.WRITER>(0) : default;
                    set => Link(0, value);
                }
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xe33b082df6033d1cUL)]
    public class Hello : ICapnpSerializable
    {
        public const UInt64 typeId = 0xe33b082df6033d1cUL;
        void ICapnpSerializable.Deserialize(DeserializerState arg_)
        {
            var reader = READER.create(arg_);
            ProtocolVersion = reader.ProtocolVersion;
            ModVersion = reader.ModVersion;
            PlayerName = reader.PlayerName;
            applyDefaults();
        }

        public void serialize(WRITER writer)
        {
            writer.ProtocolVersion = ProtocolVersion;
            writer.ModVersion = ModVersion;
            writer.PlayerName = PlayerName;
        }

        void ICapnpSerializable.Serialize(SerializerState arg_)
        {
            serialize(arg_.Rewrap<WRITER>());
        }

        public void applyDefaults()
        {
        }

        public ushort ProtocolVersion
        {
            get;
            set;
        }

        public string ModVersion
        {
            get;
            set;
        }

        public string PlayerName
        {
            get;
            set;
        }

        public struct READER
        {
            readonly DeserializerState ctx;
            public READER(DeserializerState ctx)
            {
                this.ctx = ctx;
            }

            public static READER create(DeserializerState ctx) => new READER(ctx);
            public static implicit operator DeserializerState(READER reader) => reader.ctx;
            public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
            public ushort ProtocolVersion => ctx.ReadDataUShort(0UL, (ushort)0);
            public string ModVersion => ctx.ReadText(0, null);
            public string PlayerName => ctx.ReadText(1, null);
        }

        public class WRITER : SerializerState
        {
            public WRITER()
            {
                this.SetStruct(1, 2);
            }

            public ushort ProtocolVersion
            {
                get => this.ReadDataUShort(0UL, (ushort)0);
                set => this.WriteData(0UL, value, (ushort)0);
            }

            public string ModVersion
            {
                get => this.ReadText(0, null);
                set => this.WriteText(0, value, null);
            }

            public string PlayerName
            {
                get => this.ReadText(1, null);
                set => this.WriteText(1, value, null);
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x97d0a4d5fb9a39a6UL)]
    public class Welcome : ICapnpSerializable
    {
        public const UInt64 typeId = 0x97d0a4d5fb9a39a6UL;
        void ICapnpSerializable.Deserialize(DeserializerState arg_)
        {
            var reader = READER.create(arg_);
            PlayerId = reader.PlayerId;
            SendRateHz = reader.SendRateHz;
            applyDefaults();
        }

        public void serialize(WRITER writer)
        {
            writer.PlayerId = PlayerId;
            writer.SendRateHz = SendRateHz;
        }

        void ICapnpSerializable.Serialize(SerializerState arg_)
        {
            serialize(arg_.Rewrap<WRITER>());
        }

        public void applyDefaults()
        {
        }

        public uint PlayerId
        {
            get;
            set;
        }

        public ushort SendRateHz
        {
            get;
            set;
        }

        public struct READER
        {
            readonly DeserializerState ctx;
            public READER(DeserializerState ctx)
            {
                this.ctx = ctx;
            }

            public static READER create(DeserializerState ctx) => new READER(ctx);
            public static implicit operator DeserializerState(READER reader) => reader.ctx;
            public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
            public uint PlayerId => ctx.ReadDataUInt(0UL, 0U);
            public ushort SendRateHz => ctx.ReadDataUShort(32UL, (ushort)0);
        }

        public class WRITER : SerializerState
        {
            public WRITER()
            {
                this.SetStruct(1, 0);
            }

            public uint PlayerId
            {
                get => this.ReadDataUInt(0UL, 0U);
                set => this.WriteData(0UL, value, 0U);
            }

            public ushort SendRateHz
            {
                get => this.ReadDataUShort(32UL, (ushort)0);
                set => this.WriteData(32UL, value, (ushort)0);
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xd3e827f3dc3334cbUL)]
    public class Reject : ICapnpSerializable
    {
        public const UInt64 typeId = 0xd3e827f3dc3334cbUL;
        void ICapnpSerializable.Deserialize(DeserializerState arg_)
        {
            var reader = READER.create(arg_);
            Reason = reader.Reason;
            Detail = reader.Detail;
            applyDefaults();
        }

        public void serialize(WRITER writer)
        {
            writer.Reason = Reason;
            writer.Detail = Detail;
        }

        void ICapnpSerializable.Serialize(SerializerState arg_)
        {
            serialize(arg_.Rewrap<WRITER>());
        }

        public void applyDefaults()
        {
        }

        public CapnpGen.Reject.RejectReason Reason
        {
            get;
            set;
        }

        public string Detail
        {
            get;
            set;
        }

        public struct READER
        {
            readonly DeserializerState ctx;
            public READER(DeserializerState ctx)
            {
                this.ctx = ctx;
            }

            public static READER create(DeserializerState ctx) => new READER(ctx);
            public static implicit operator DeserializerState(READER reader) => reader.ctx;
            public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
            public CapnpGen.Reject.RejectReason Reason => (CapnpGen.Reject.RejectReason)ctx.ReadDataUShort(0UL, (ushort)0);
            public string Detail => ctx.ReadText(0, null);
        }

        public class WRITER : SerializerState
        {
            public WRITER()
            {
                this.SetStruct(1, 1);
            }

            public CapnpGen.Reject.RejectReason Reason
            {
                get => (CapnpGen.Reject.RejectReason)this.ReadDataUShort(0UL, (ushort)0);
                set => this.WriteData(0UL, (ushort)value, (ushort)0);
            }

            public string Detail
            {
                get => this.ReadText(0, null);
                set => this.WriteText(0, value, null);
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xdbc8abfd8ad6fa1eUL)]
        public enum RejectReason : ushort
        {
            versionMismatch,
            serverFull,
            kicked,
            other
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xc7191d40c31b19beUL)]
    public class PlayerInfo : ICapnpSerializable
    {
        public const UInt64 typeId = 0xc7191d40c31b19beUL;
        void ICapnpSerializable.Deserialize(DeserializerState arg_)
        {
            var reader = READER.create(arg_);
            PlayerId = reader.PlayerId;
            Name = reader.Name;
            Zone = reader.Zone;
            applyDefaults();
        }

        public void serialize(WRITER writer)
        {
            writer.PlayerId = PlayerId;
            writer.Name = Name;
            writer.Zone = Zone;
        }

        void ICapnpSerializable.Serialize(SerializerState arg_)
        {
            serialize(arg_.Rewrap<WRITER>());
        }

        public void applyDefaults()
        {
        }

        public uint PlayerId
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public string Zone
        {
            get;
            set;
        }

        public struct READER
        {
            readonly DeserializerState ctx;
            public READER(DeserializerState ctx)
            {
                this.ctx = ctx;
            }

            public static READER create(DeserializerState ctx) => new READER(ctx);
            public static implicit operator DeserializerState(READER reader) => reader.ctx;
            public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
            public uint PlayerId => ctx.ReadDataUInt(0UL, 0U);
            public string Name => ctx.ReadText(0, null);
            public string Zone => ctx.ReadText(1, null);
        }

        public class WRITER : SerializerState
        {
            public WRITER()
            {
                this.SetStruct(1, 2);
            }

            public uint PlayerId
            {
                get => this.ReadDataUInt(0UL, 0U);
                set => this.WriteData(0UL, value, 0U);
            }

            public string Name
            {
                get => this.ReadText(0, null);
                set => this.WriteText(0, value, null);
            }

            public string Zone
            {
                get => this.ReadText(1, null);
                set => this.WriteText(1, value, null);
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xf843bdb0c97d923fUL)]
    public class PlayerTransform : ICapnpSerializable
    {
        public const UInt64 typeId = 0xf843bdb0c97d923fUL;
        void ICapnpSerializable.Deserialize(DeserializerState arg_)
        {
            var reader = READER.create(arg_);
            PlayerId = reader.PlayerId;
            X = reader.X;
            Y = reader.Y;
            Z = reader.Z;
            Yaw = reader.Yaw;
            Vx = reader.Vx;
            Vy = reader.Vy;
            Vz = reader.Vz;
            Anim = reader.Anim;
            applyDefaults();
        }

        public void serialize(WRITER writer)
        {
            writer.PlayerId = PlayerId;
            writer.X = X;
            writer.Y = Y;
            writer.Z = Z;
            writer.Yaw = Yaw;
            writer.Vx = Vx;
            writer.Vy = Vy;
            writer.Vz = Vz;
            writer.Anim = Anim;
        }

        void ICapnpSerializable.Serialize(SerializerState arg_)
        {
            serialize(arg_.Rewrap<WRITER>());
        }

        public void applyDefaults()
        {
        }

        public uint PlayerId
        {
            get;
            set;
        }

        public float X
        {
            get;
            set;
        }

        public float Y
        {
            get;
            set;
        }

        public float Z
        {
            get;
            set;
        }

        public float Yaw
        {
            get;
            set;
        }

        public float Vx
        {
            get;
            set;
        }

        public float Vy
        {
            get;
            set;
        }

        public float Vz
        {
            get;
            set;
        }

        public CapnpGen.PlayerTransform.AnimState Anim
        {
            get;
            set;
        }

        public struct READER
        {
            readonly DeserializerState ctx;
            public READER(DeserializerState ctx)
            {
                this.ctx = ctx;
            }

            public static READER create(DeserializerState ctx) => new READER(ctx);
            public static implicit operator DeserializerState(READER reader) => reader.ctx;
            public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
            public uint PlayerId => ctx.ReadDataUInt(0UL, 0U);
            public float X => ctx.ReadDataFloat(32UL, 0F);
            public float Y => ctx.ReadDataFloat(64UL, 0F);
            public float Z => ctx.ReadDataFloat(96UL, 0F);
            public float Yaw => ctx.ReadDataFloat(128UL, 0F);
            public float Vx => ctx.ReadDataFloat(160UL, 0F);
            public float Vy => ctx.ReadDataFloat(192UL, 0F);
            public float Vz => ctx.ReadDataFloat(224UL, 0F);
            public CapnpGen.PlayerTransform.AnimState Anim => (CapnpGen.PlayerTransform.AnimState)ctx.ReadDataUShort(256UL, (ushort)0);
        }

        public class WRITER : SerializerState
        {
            public WRITER()
            {
                this.SetStruct(5, 0);
            }

            public uint PlayerId
            {
                get => this.ReadDataUInt(0UL, 0U);
                set => this.WriteData(0UL, value, 0U);
            }

            public float X
            {
                get => this.ReadDataFloat(32UL, 0F);
                set => this.WriteData(32UL, value, 0F);
            }

            public float Y
            {
                get => this.ReadDataFloat(64UL, 0F);
                set => this.WriteData(64UL, value, 0F);
            }

            public float Z
            {
                get => this.ReadDataFloat(96UL, 0F);
                set => this.WriteData(96UL, value, 0F);
            }

            public float Yaw
            {
                get => this.ReadDataFloat(128UL, 0F);
                set => this.WriteData(128UL, value, 0F);
            }

            public float Vx
            {
                get => this.ReadDataFloat(160UL, 0F);
                set => this.WriteData(160UL, value, 0F);
            }

            public float Vy
            {
                get => this.ReadDataFloat(192UL, 0F);
                set => this.WriteData(192UL, value, 0F);
            }

            public float Vz
            {
                get => this.ReadDataFloat(224UL, 0F);
                set => this.WriteData(224UL, value, 0F);
            }

            public CapnpGen.PlayerTransform.AnimState Anim
            {
                get => (CapnpGen.PlayerTransform.AnimState)this.ReadDataUShort(256UL, (ushort)0);
                set => this.WriteData(256UL, (ushort)value, (ushort)0);
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xf8216f42b6b34acdUL)]
        public enum AnimState : ushort
        {
            idle,
            run,
            jump,
            fall,
            glide,
            strider,
            hairpin,
            attack,
            hurt,
            dead
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x8ce411f61d88d1b4UL)]
    public class ZoneChange : ICapnpSerializable
    {
        public const UInt64 typeId = 0x8ce411f61d88d1b4UL;
        void ICapnpSerializable.Deserialize(DeserializerState arg_)
        {
            var reader = READER.create(arg_);
            PlayerId = reader.PlayerId;
            Zone = reader.Zone;
            applyDefaults();
        }

        public void serialize(WRITER writer)
        {
            writer.PlayerId = PlayerId;
            writer.Zone = Zone;
        }

        void ICapnpSerializable.Serialize(SerializerState arg_)
        {
            serialize(arg_.Rewrap<WRITER>());
        }

        public void applyDefaults()
        {
        }

        public uint PlayerId
        {
            get;
            set;
        }

        public string Zone
        {
            get;
            set;
        }

        public struct READER
        {
            readonly DeserializerState ctx;
            public READER(DeserializerState ctx)
            {
                this.ctx = ctx;
            }

            public static READER create(DeserializerState ctx) => new READER(ctx);
            public static implicit operator DeserializerState(READER reader) => reader.ctx;
            public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
            public uint PlayerId => ctx.ReadDataUInt(0UL, 0U);
            public string Zone => ctx.ReadText(0, null);
        }

        public class WRITER : SerializerState
        {
            public WRITER()
            {
                this.SetStruct(1, 1);
            }

            public uint PlayerId
            {
                get => this.ReadDataUInt(0UL, 0U);
                set => this.WriteData(0UL, value, 0U);
            }

            public string Zone
            {
                get => this.ReadText(0, null);
                set => this.WriteText(0, value, null);
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x89c9b8cc45396944UL)]
    public class PlayerState : ICapnpSerializable
    {
        public const UInt64 typeId = 0x89c9b8cc45396944UL;
        void ICapnpSerializable.Deserialize(DeserializerState arg_)
        {
            var reader = READER.create(arg_);
            Info = CapnpSerializable.Create<CapnpGen.PlayerInfo>(reader.Info);
            Transform = CapnpSerializable.Create<CapnpGen.PlayerTransform>(reader.Transform);
            applyDefaults();
        }

        public void serialize(WRITER writer)
        {
            Info?.serialize(writer.Info);
            Transform?.serialize(writer.Transform);
        }

        void ICapnpSerializable.Serialize(SerializerState arg_)
        {
            serialize(arg_.Rewrap<WRITER>());
        }

        public void applyDefaults()
        {
        }

        public CapnpGen.PlayerInfo Info
        {
            get;
            set;
        }

        public CapnpGen.PlayerTransform Transform
        {
            get;
            set;
        }

        public struct READER
        {
            readonly DeserializerState ctx;
            public READER(DeserializerState ctx)
            {
                this.ctx = ctx;
            }

            public static READER create(DeserializerState ctx) => new READER(ctx);
            public static implicit operator DeserializerState(READER reader) => reader.ctx;
            public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
            public CapnpGen.PlayerInfo.READER Info => ctx.ReadStruct(0, CapnpGen.PlayerInfo.READER.create);
            public CapnpGen.PlayerTransform.READER Transform => ctx.ReadStruct(1, CapnpGen.PlayerTransform.READER.create);
        }

        public class WRITER : SerializerState
        {
            public WRITER()
            {
                this.SetStruct(0, 2);
            }

            public CapnpGen.PlayerInfo.WRITER Info
            {
                get => BuildPointer<CapnpGen.PlayerInfo.WRITER>(0);
                set => Link(0, value);
            }

            public CapnpGen.PlayerTransform.WRITER Transform
            {
                get => BuildPointer<CapnpGen.PlayerTransform.WRITER>(1);
                set => Link(1, value);
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x93b14e2f9174a6d6UL)]
    public class Snapshot : ICapnpSerializable
    {
        public const UInt64 typeId = 0x93b14e2f9174a6d6UL;
        void ICapnpSerializable.Deserialize(DeserializerState arg_)
        {
            var reader = READER.create(arg_);
            Players = reader.Players?.ToReadOnlyList(_ => CapnpSerializable.Create<CapnpGen.PlayerState>(_));
            applyDefaults();
        }

        public void serialize(WRITER writer)
        {
            writer.Players.Init(Players, (_s1, _v1) => _v1?.serialize(_s1));
        }

        void ICapnpSerializable.Serialize(SerializerState arg_)
        {
            serialize(arg_.Rewrap<WRITER>());
        }

        public void applyDefaults()
        {
        }

        public IReadOnlyList<CapnpGen.PlayerState> Players
        {
            get;
            set;
        }

        public struct READER
        {
            readonly DeserializerState ctx;
            public READER(DeserializerState ctx)
            {
                this.ctx = ctx;
            }

            public static READER create(DeserializerState ctx) => new READER(ctx);
            public static implicit operator DeserializerState(READER reader) => reader.ctx;
            public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
            public IReadOnlyList<CapnpGen.PlayerState.READER> Players => ctx.ReadList(0).Cast(CapnpGen.PlayerState.READER.create);
        }

        public class WRITER : SerializerState
        {
            public WRITER()
            {
                this.SetStruct(0, 1);
            }

            public ListOfStructsSerializer<CapnpGen.PlayerState.WRITER> Players
            {
                get => BuildPointer<ListOfStructsSerializer<CapnpGen.PlayerState.WRITER>>(0);
                set => Link(0, value);
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xea063a4985966990UL)]
    public class ChatBroadcast : ICapnpSerializable
    {
        public const UInt64 typeId = 0xea063a4985966990UL;
        void ICapnpSerializable.Deserialize(DeserializerState arg_)
        {
            var reader = READER.create(arg_);
            PlayerId = reader.PlayerId;
            Text = reader.Text;
            applyDefaults();
        }

        public void serialize(WRITER writer)
        {
            writer.PlayerId = PlayerId;
            writer.Text = Text;
        }

        void ICapnpSerializable.Serialize(SerializerState arg_)
        {
            serialize(arg_.Rewrap<WRITER>());
        }

        public void applyDefaults()
        {
        }

        public uint PlayerId
        {
            get;
            set;
        }

        public string Text
        {
            get;
            set;
        }

        public struct READER
        {
            readonly DeserializerState ctx;
            public READER(DeserializerState ctx)
            {
                this.ctx = ctx;
            }

            public static READER create(DeserializerState ctx) => new READER(ctx);
            public static implicit operator DeserializerState(READER reader) => reader.ctx;
            public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
            public uint PlayerId => ctx.ReadDataUInt(0UL, 0U);
            public string Text => ctx.ReadText(0, null);
        }

        public class WRITER : SerializerState
        {
            public WRITER()
            {
                this.SetStruct(1, 1);
            }

            public uint PlayerId
            {
                get => this.ReadDataUInt(0UL, 0U);
                set => this.WriteData(0UL, value, 0U);
            }

            public string Text
            {
                get => this.ReadText(0, null);
                set => this.WriteText(0, value, null);
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xd452eb6036ba473dUL)]
    public class PlayerEvent : ICapnpSerializable
    {
        public const UInt64 typeId = 0xd452eb6036ba473dUL;
        void ICapnpSerializable.Deserialize(DeserializerState arg_)
        {
            var reader = READER.create(arg_);
            PlayerId = reader.PlayerId;
            Kind = CapnpSerializable.Create<CapnpGen.PlayerEvent.kind>(reader.Kind);
            applyDefaults();
        }

        public void serialize(WRITER writer)
        {
            writer.PlayerId = PlayerId;
            Kind?.serialize(writer.Kind);
        }

        void ICapnpSerializable.Serialize(SerializerState arg_)
        {
            serialize(arg_.Rewrap<WRITER>());
        }

        public void applyDefaults()
        {
        }

        public uint PlayerId
        {
            get;
            set;
        }

        public CapnpGen.PlayerEvent.kind Kind
        {
            get;
            set;
        }

        public struct READER
        {
            readonly DeserializerState ctx;
            public READER(DeserializerState ctx)
            {
                this.ctx = ctx;
            }

            public static READER create(DeserializerState ctx) => new READER(ctx);
            public static implicit operator DeserializerState(READER reader) => reader.ctx;
            public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
            public uint PlayerId => ctx.ReadDataUInt(0UL, 0U);
            public kind.READER Kind => new kind.READER(ctx);
        }

        public class WRITER : SerializerState
        {
            public WRITER()
            {
                this.SetStruct(2, 0);
            }

            public uint PlayerId
            {
                get => this.ReadDataUInt(0UL, 0U);
                set => this.WriteData(0UL, value, 0U);
            }

            public kind.WRITER Kind
            {
                get => Rewrap<kind.WRITER>();
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xfbeaa51ea05c7c28UL)]
        public class kind : ICapnpSerializable
        {
            public const UInt64 typeId = 0xfbeaa51ea05c7c28UL;
            public enum WHICH : ushort
            {
                Attack = 0,
                Hit = 1,
                Died = 2,
                Respawned = 3,
                undefined = 65535
            }

            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                switch (reader.which)
                {
                    case WHICH.Attack:
                        which = reader.which;
                        break;
                    case WHICH.Hit:
                        Hit = reader.Hit;
                        break;
                    case WHICH.Died:
                        which = reader.which;
                        break;
                    case WHICH.Respawned:
                        which = reader.which;
                        break;
                }

                applyDefaults();
            }

            private WHICH _which = WHICH.undefined;
            private object _content;
            public WHICH which
            {
                get => _which;
                set
                {
                    if (value == _which)
                        return;
                    _which = value;
                    switch (value)
                    {
                        case WHICH.Attack:
                            break;
                        case WHICH.Hit:
                            _content = 0F;
                            break;
                        case WHICH.Died:
                            break;
                        case WHICH.Respawned:
                            break;
                    }
                }
            }

            public void serialize(WRITER writer)
            {
                writer.which = which;
                switch (which)
                {
                    case WHICH.Attack:
                        break;
                    case WHICH.Hit:
                        writer.Hit = Hit.Value;
                        break;
                    case WHICH.Died:
                        break;
                    case WHICH.Respawned:
                        break;
                }
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public float? Hit
            {
                get => _which == WHICH.Hit ? (float? )_content : null;
                set
                {
                    _which = WHICH.Hit;
                    _content = value;
                }
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
                public WHICH which => (WHICH)ctx.ReadDataUShort(32U, (ushort)0);
                public float Hit => which == WHICH.Hit ? ctx.ReadDataFloat(64UL, 0F) : default;
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                }

                public WHICH which
                {
                    get => (WHICH)this.ReadDataUShort(32U, (ushort)0);
                    set => this.WriteData(32U, (ushort)value, (ushort)0);
                }

                public float Hit
                {
                    get => which == WHICH.Hit ? this.ReadDataFloat(64UL, 0F) : default;
                    set => this.WriteData(64UL, value, 0F);
                }
            }
        }
    }
}