package game

import (
	"sync"
	"time"
)

// Transform is the last movement state a player reported.
type Transform struct {
	X, Y, Z, Yaw float32
	VX, VY, VZ   float32
	Anim         uint16 // messages.PlayerTransform_AnimState
}

// Player is a joined player. Values returned by Engine methods are copies.
type Player struct {
	ID         uint32
	Name       string
	Zone       string
	Transform  Transform
	JoinedAt   time.Time
	LastUpdate time.Time
}

// Engine owns all game state. It is safe to call from multiple goroutines
// (the network loop writes, the tick loop reads).
type Engine struct {
	mu      sync.RWMutex
	players map[uint32]*Player
	nextID  uint32

	ticker *time.Ticker
	stop   chan struct{}
}

func NewEngine(tickRate int) *Engine {
	return &Engine{
		players: make(map[uint32]*Player),
		nextID:  1, // 0 is reserved for "server"
		ticker:  time.NewTicker(time.Second / time.Duration(tickRate)),
		stop:    make(chan struct{}),
	}
}

func (e *Engine) Start() {
	for {
		select {
		case <-e.stop:
			return
		case <-e.ticker.C:
			e.tick()
		}
	}
}

func (e *Engine) Stop() {
	e.ticker.Stop()
	close(e.stop)
}

func (e *Engine) tick() {
	// Server-side simulation goes here later (timeouts, authoritative state, etc.).
}

// AddPlayer registers a new player and returns it with its assigned ID.
func (e *Engine) AddPlayer(name string) Player {
	e.mu.Lock()
	defer e.mu.Unlock()

	now := time.Now()
	p := &Player{ID: e.nextID, Name: name, JoinedAt: now, LastUpdate: now}
	e.nextID++
	e.players[p.ID] = p
	return *p
}

// RemovePlayer deletes a player and returns what was stored.
func (e *Engine) RemovePlayer(id uint32) (Player, bool) {
	e.mu.Lock()
	defer e.mu.Unlock()

	p, ok := e.players[id]
	if !ok {
		return Player{}, false
	}
	delete(e.players, id)
	return *p, true
}

func (e *Engine) UpdateTransform(id uint32, t Transform) (Player, bool) {
	e.mu.Lock()
	defer e.mu.Unlock()

	p, ok := e.players[id]
	if !ok {
		return Player{}, false
	}
	p.Transform = t
	p.LastUpdate = time.Now()
	return *p, true
}

func (e *Engine) SetZone(id uint32, zone string) (Player, bool) {
	e.mu.Lock()
	defer e.mu.Unlock()

	p, ok := e.players[id]
	if !ok {
		return Player{}, false
	}
	p.Zone = zone
	return *p, true
}

func (e *Engine) Player(id uint32) (Player, bool) {
	e.mu.RLock()
	defer e.mu.RUnlock()

	p, ok := e.players[id]
	if !ok {
		return Player{}, false
	}
	return *p, true
}

// Players returns a snapshot of every joined player.
func (e *Engine) Players() []Player {
	e.mu.RLock()
	defer e.mu.RUnlock()

	out := make([]Player, 0, len(e.players))
	for _, p := range e.players {
		out = append(out, *p)
	}
	return out
}

func (e *Engine) Count() int {
	e.mu.RLock()
	defer e.mu.RUnlock()
	return len(e.players)
}
