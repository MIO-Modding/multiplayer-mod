package main

import (
	"flag"
	"log"
	"os"
	"os/signal"
	"syscall"

	"server/internal/game"
	"server/internal/network"
)

func main() {
	port := flag.Uint("port", 8095, "UDP port to listen on")
	maxPlayers := flag.Int("max-players", 32, "maximum number of joined players")
	sendRate := flag.Uint("send-rate", 20, "transform updates per second clients are told to send")
	tickRate := flag.Int("tick-rate", 60, "server simulation ticks per second")
	modVersion := flag.String("mod-version", "", "required client mod version (empty accepts any)")
	verbose := flag.Bool("verbose", false, "log every transform update")
	flag.Parse()

	switch {
	case *port < 1 || *port > 65535:
		log.Fatalf("[main] -port must be 1-65535, got %d", *port)
	case *maxPlayers < 1 || *maxPlayers > 4000:
		log.Fatalf("[main] -max-players must be 1-4000, got %d", *maxPlayers)
	case *sendRate < 1 || *sendRate > 120:
		log.Fatalf("[main] -send-rate must be 1-120, got %d", *sendRate)
	case *tickRate < 1 || *tickRate > 1000:
		log.Fatalf("[main] -tick-rate must be 1-1000, got %d", *tickRate)
	}

	log.Println("[main] starting mio mp server...")

	// 1. Game engine / state loop
	gameEngine := game.NewEngine(*tickRate)
	go gameEngine.Start()
	defer gameEngine.Stop()

	// 2. ENet server layer
	server, err := network.NewServer(network.Config{
		Port:       uint16(*port),
		MaxPlayers: *maxPlayers,
		SendRateHz: uint16(*sendRate),
		ModVersion: *modVersion,
		Verbose:    *verbose,
	}, gameEngine)
	if err != nil {
		log.Fatalf("[main] failed to initialize enet server: %v", err)
	}
	defer server.Shutdown()

	go server.ListenLoop()

	// 3. Graceful shutdown
	stop := make(chan os.Signal, 1)
	signal.Notify(stop, os.Interrupt, syscall.SIGTERM)
	<-stop

	log.Println("[main] shutting down server safely...")
}
