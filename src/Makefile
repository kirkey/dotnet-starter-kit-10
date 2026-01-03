SHELL := /bin/zsh
# Usage:
# make help
# make setup                  # Setup HTTPS certificate and trust it
# make apphost                # Run with .NET Aspire (includes Redis, Postgres)
# make api DOTNET_RUN_ARGS="--urls https://localhost:5001"
# make blazor
# make nswag
# make kill-ports DRY_RUN=1    # preview ports and PIDs without killing
DOTNET_RUN_ARGS ?=
DRY_RUN ?= 0
ASPIRE_RUN ?= 0
# Find NSwag config files (.nswag, nswag.json, nswag.*.json)
NSWAG_FILES := $(shell find . -type f \( -iname '*.nswag' -o -iname 'nswag*.json' \) -print 2>/dev/null | tr '\n' ' ')
# Extract ports from any applicationUrl entries in launchSettings.json (handles multiple URLs separated by ';')
# Properly extract port numbers from URLs like "https://localhost:7030;http://localhost:5030"
EXTRACTED_PORTS := $(shell grep -rh '"applicationUrl"' . 2>/dev/null | grep -o ':[0-9]\+' | grep -o '[0-9]\+' | sort -u | tr '\n' ' ')
.PHONY: help setup apphost api api-http blazor blazor-http nswag kill-ports
help:
	@echo "Makefile targets:"
	@echo ""
	@echo "ASPIRE (with .NET Aspire orchestration, Redis, PostgreSQL):"
	@echo "  apphost     - Run .NET Aspire AppHost (orchestrates API, Blazor, Redis, Postgres)"
	@echo ""
	@echo "STANDALONE (without Aspire - simpler for development):"
	@echo "  setup       - Setup HTTPS developer certificate (run once)"
	@echo "  api         - Run API with HTTPS (ports 7030 + 5030)"
	@echo "  api-http    - Run API with HTTP only (port 5030) - recommended"
	@echo "  blazor      - Run Blazor with HTTPS (ports 7140 + 5032)"
	@echo "  blazor-http - Run Blazor with HTTP only (port 5032) - recommended"
	@echo "  nswag       - Run NSwag code generator"
	@echo "  kill-ports  - Kill processes on detected ports"
	@echo ""
	@echo "Examples:"
	@echo "  make apphost             # Run full Aspire stack (recommended for production-like setup)"
	@echo "  make api-http && make blazor-http  # Quick development setup (two terminals)"
	@echo "  make kill-ports DRY_RUN=1 # Preview ports before killing"
setup:
	@echo "Setting up HTTPS developer certificate..."
	@dotnet dev-certs https --check --trust || dotnet dev-certs https --trust
	@echo "HTTPS certificate is now set up and trusted!"

apphost:
	@echo "🚀 Starting .NET Aspire AppHost..."
	@echo "This will start:"
	@echo "  - PostgreSQL database (localhost)"
	@echo "  - Redis cache (localhost)"
	@echo "  - Apps API"
	@echo "  - Apps Blazor UI"
	@echo ""
	@echo "The Aspire Dashboard will open automatically."
	@echo "Press Ctrl+C to stop all services."
	@echo ""
	@dotnet run --project Apps/FSH.Apps.AppHost -- $(DOTNET_RUN_ARGS)
api:
	@echo "Running Apps.Api (HTTPS on 7030, HTTP on 5030)"
	@dotnet run --project Apps/Apps.Api -- $(DOTNET_RUN_ARGS)
api-http:
	@echo "Running Apps.Api (HTTP only on port 5030)"
	@ASPNETCORE_URLS="http://localhost:5030" dotnet run --project Apps/Apps.Api -- $(DOTNET_RUN_ARGS)
blazor:
	@echo "Running Apps.Blazor (HTTPS on 7140, HTTP on 5032)"
	@dotnet run --project Apps/Apps.Blazor -- $(DOTNET_RUN_ARGS)
blazor-http:
	@echo "Running Apps.Blazor (HTTP only on port 5032)"
	@ASPNETCORE_URLS="http://localhost:5032" dotnet run --project Apps/Apps.Blazor -- $(DOTNET_RUN_ARGS)
nswag:
	@if [ -z "$(NSWAG_FILES)" ]; then \
		echo "No NSwag config files found (looked for .nswag and nswag*.json)."; \
		echo "Install NSwag CLI: dotnet tool install --global NSwag.ConsoleCore"; \
		exit 1; \
	fi; \
	if command -v nswag >/dev/null 2>&1; then \
		RUNNER="nswag"; \
	elif dotnet tool list -g 2>/dev/null | grep -qi nswag; then \
		RUNNER="dotnet tool run nswag"; \
	else \
		echo "NSwag not found. Install with: dotnet tool install --global NSwag.ConsoleCore"; exit 1; \
	fi; \
	for f in $(NSWAG_FILES); do \
		echo "Running $$RUNNER run $$f"; \
		$$RUNNER run "$$f" /variables:SpecUrl=https://localhost:7030/openapi/v1.json || { echo "NSwag failed for $$f"; exit 1; }; \
	done
kill-ports:
	@echo "DRY_RUN=$(DRY_RUN)"
	@if [ -z "$(EXTRACTED_PORTS)" ]; then \
		echo "No ports found in launchSettings.json/applicationUrl entries."; \
		exit 0; \
	fi; \
	echo "Extracted ports: $(EXTRACTED_PORTS)"; \
	for p in $(EXTRACTED_PORTS); do \
		echo "Checking port $$p..."; \
		PIDS=$$(lsof -ti tcp:$$p 2>/dev/null || true); \
		if [ -z "$$PIDS" ]; then \
			echo "  No process listening on port $$p"; \
			continue; \
		fi; \
		echo "  Found PIDs: $$PIDS"; \
		if [ "$(DRY_RUN)" = "1" ]; then \
			echo "  DRY RUN - not killing"; \
			continue; \
		fi; \
		echo "  Killing..."; \
		echo "$$PIDS" | while read -r pid; do \
			kill -9 $$pid 2>/dev/null || echo "    Failed to kill $$pid"; \
		done; \
	done
	@echo "Done."
