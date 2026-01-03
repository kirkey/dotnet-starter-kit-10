.PHONY: help docker-up docker-down docker-restart clean build test run

help:
	@echo "FSH Framework Development Commands"
	@echo "==================================="
	@echo "make docker-up          - Start Redis, PostgreSQL, and Jaeger"
	@echo "make docker-down        - Stop and remove containers"
	@echo "make docker-restart     - Restart containers"
	@echo "make redis-cli          - Connect to Redis CLI"
	@echo "make psql               - Connect to PostgreSQL"
	@echo "make build              - Build the solution"
	@echo "make test               - Run unit tests"
	@echo "make api                - Run the API project"
	@echo "make blazor             - Run the Blazor UI project"
	@echo "make clean              - Clean build artifacts"
	@echo "make kill-ports         - Kill processes on default ports"

# Docker commands
docker-up:
	docker-compose -f docker-compose.yml up -d
	@echo "✓ Services started: Redis (6379), PostgreSQL (5432), Jaeger (4317/16686)"

docker-down:
	docker-compose -f docker-compose.yml down
	@echo "✓ Services stopped"

docker-restart: docker-down docker-up
	@echo "✓ Services restarted"

docker-logs:
	docker-compose -f docker-compose.yml logs -f

redis-cli:
	docker exec -it fsh-redis-dev redis-cli

psql:
	docker exec -it fsh-postgres-dev psql -U postgres -d fsh_db

# Build and Test
build:
	cd src && dotnet build -c Debug

release-build:
	cd src && dotnet build -c Release

test:
	cd src && dotnet test

clean:
	cd src && dotnet clean
	find . -type d -name "bin" -o -name "obj" | xargs rm -rf

# Run applications
api:
	cd src/Apps/Apps.Api && dotnet run --configuration Debug

blazor:
	cd src/Apps/Apps.Blazor && dotnet run --configuration Debug

# Utilities
kill-ports:
	@echo "Killing processes on ports: 6379 (Redis), 5432 (PostgreSQL), 7030 (API), 7140 (Blazor), 4317 (Jaeger)..."
	@lsof -ti:6379,5432,7030,7140,4317 | xargs kill -9 2>/dev/null || true
	@echo "✓ Ports cleared"

dev-setup: docker-up build
	@echo "✓ Development environment setup complete!"
	@echo "  - Redis running on localhost:6379"
	@echo "  - PostgreSQL running on localhost:5432"
	@echo "  - Jaeger UI available at http://localhost:16686"
	@echo ""
	@echo "Next steps:"
	@echo "  1. Run 'make api' to start the API server"
	@echo "  2. Run 'make blazor' to start the Blazor UI"

migrate:
	cd src && dotnet ef database update --project Apps/Migrations.PostgreSQL/Apps.Migrations.PostgreSQL.csproj --startup-project Apps/Apps.Api/Apps.Api.csproj

