@0x81b63525c602dba3;

using Go = import "/go.capnp";
$Go.package("messages");
$Go.import("shared/gen/go/messages");

# bump when the protocol changes in a way old clients can't handle.
const currentProtocolVersion :UInt16 = 1;

# every ENet packet carries exactly one NetworkPacket
struct NetworkPacket {
  id @0 :UInt32; # per-sender sequence number
  timestamp @1 :Int64; # sender's unix time in ms

  body :union {
    # --- keep alive ---
    ping @2 :Void;
    pong @3 :Void;

    # --- movement (client -> server, server rebroadcasts; send unreliable) ---
    playerTransform @4 :PlayerTransform;

    # --- chat ---
    chatMessage @5 :Text; # client -> server
    chatBroadcast @6 :ChatBroadcast; # server -> clients

    # --- session ---
    hello @7 :Hello;               # client -> server, first packet after connecting
    welcome @8 :Welcome;           # server -> client, reply to hello
    reject @9 :Reject;             # server -> client, before disconnecting it
    playerJoined @10 :PlayerInfo;  # server -> clients
    playerLeft @11 :UInt32;        # server -> clients, playerId

    # --- world ---
    zoneChange @12 :ZoneChange;    # client -> server, server rebroadcasts
    snapshot @13 :Snapshot;        # server -> client, sent right after welcome

    # --- gameplay events (client -> server, server rebroadcasts) ---
    playerEvent @14 :PlayerEvent;
  }
}

# ---------- session ----------

struct Hello {
  protocolVersion @0 :UInt16;
  modVersion @1 :Text;
  playerName @2 :Text;
}

struct Welcome {
  playerId @0 :UInt32;     # assigned by the server
  sendRateHz @1 :UInt16;   # how often the client should send PlayerTransform
}

struct Reject {
  reason @0 :RejectReason;
  detail @1 :Text;

  enum RejectReason {
    versionMismatch @0;
    serverFull @1;
    kicked @2;
    other @3;
  }
}

struct PlayerInfo {
  playerId @0 :UInt32;
  name @1 :Text;
  zone @2 :Text;
}

# ---------- movement ----------

struct PlayerTransform {
  playerId @0 :UInt32;   # ignored by the server on input; filled in on rebroadcast
  x @1 :Float32;
  y @2 :Float32;
  z @3 :Float32;
  yaw @4 :Float32;
  vx @5 :Float32;        # velocity, for interpolation/extrapolation
  vy @6 :Float32;
  vz @7 :Float32;
  anim @8 :AnimState;

  enum AnimState {
    idle @0;
    run @1;
    jump @2;
    fall @3;
    glide @4;
    strider @5;
    hairpin @6;
    attack @7;
    hurt @8;
    dead @9;
  }
}

# ---------- world ----------

struct ZoneChange {
  playerId @0 :UInt32;
  zone @1 :Text;
}

struct PlayerState {
  info @0 :PlayerInfo;
  transform @1 :PlayerTransform;
}

struct Snapshot {
  players @0 :List(PlayerState);
}

# ---------- chat ----------

struct ChatBroadcast {
  playerId @0 :UInt32;   # 0 = server message
  text @1 :Text;
}

# ---------- gameplay ----------

struct PlayerEvent {
  playerId @0 :UInt32;

  kind :union {
    attack @1 :Void;
    hit @2 :Float32;     # damage taken
    died @3 :Void;
    respawned @4 :Void;
  }
}
