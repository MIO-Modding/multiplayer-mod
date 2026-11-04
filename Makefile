.PHONY: generate clean

GO_CAPNP_STD := $(shell go list -m -f '{{.Dir}}' capnproto.org/go/capnp/v3)/std

generate:
	@echo "generating cap'n proto targets..."
	@mkdir -p shared/gen/go/messages client/Src/Generated
	DOTNET_ROLL_FORWARD=Major capnp compile -I"$(GO_CAPNP_STD)" --src-prefix=shared/proto \
		-ogo:shared/gen/go/messages \
		-ocsharp:client/Src/Generated \
		shared/proto/game.capnp
	@echo "generation complete!"


clean:
	@echo "cleaning generated outputs..."
	rm -rf shared/gen/go/*
	rm -rf client/Src/Generated/*