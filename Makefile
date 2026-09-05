# FullStackHero .NET Starter Kit — common workflows.
# Run `make help` to list targets. See AGENTS.md for the full contributor guide.

SLNX       := src/FSH.Starter.slnx
API        := src/Host/FSH.Starter.Api
APPHOST    := src/Host/FSH.Starter.AppHost
MIGRATOR   := src/Host/FSH.Starter.DbMigrator
MIGRATIONS := src/Host/FSH.Starter.Migrations.PostgreSQL
CONFIG      ?= Debug
TEST_CONFIG ?= Release
# Every port this repo binds: API, both apps, Aspire dashboard, Postgres,
# pgAdmin, Valkey, MinIO. `make kill` frees them all (requires lsof).
KILL_PORTS  := 7030 5030 5173 5174 15888 5432 5050 6379 9000 9001
UNIT_TESTS  := Architecture Auditing Caching Generic Identity Multitenancy Billing Catalog Chat Files Framework Webhooks

.PHONY: help
help: ## Show this help
	@grep -E '^[a-zA-Z0-9_.-]+:.*?## ' $(MAKEFILE_LIST) | sort | awk 'BEGIN {FS = ":.*?## "}; {printf "  \033[36m%-22s\033[0m %s\n", $$1, $$2}'

.PHONY: restore tools install
restore: ## Restore NuGet packages + local dotnet tools
	dotnet restore $(SLNX)
	dotnet tool restore

tools: ## Restore local dotnet tools only (dotnet-ef, fsh)
	dotnet tool restore

install: ## npm install both React apps (one-time setup)
	cd clients/admin && npm install
	cd clients/dashboard && npm install

.PHONY: build
build: ## Build the backend solution
	dotnet build $(SLNX) -c $(CONFIG)

.PHONY: run-all run-api run-ui run-ui-admin run-ui-dashboard
run-all: ## Whole stack via Aspire (API + apps + Postgres + Valkey + MinIO)
	dotnet run --project $(APPHOST)

run-api: ## API only → https://localhost:7030 (/scalar)
	dotnet run --project $(API)

run-ui-admin: ## Admin dev server → http://localhost:5173
	cd clients/admin && npm run dev

run-ui-dashboard: ## Dashboard dev server → http://localhost:5174
	cd clients/dashboard && npm run dev

run-ui: ## Both React dev servers (Ctrl-C stops both)
	cd clients/admin && npm run dev & \
	cd clients/dashboard && npm run dev & \
	wait

.PHONY: kill-ports
kill-ports: ## Kill processes listening on the repo's ports (API, apps, infra)
	for port in $(KILL_PORTS); do \
		pids=$$(lsof -ti tcp:$${port} 2>/dev/null) || true; \
		if [ -n "$$pids" ]; then \
			echo "killing :$${port} ($$pids)"; \
			kill $$pids 2>/dev/null || true; \
		fi; \
	done

.PHONY: migrate seed seed-demo migrate-pending migration
# NOTE: DbMigrator has no launchSettings, so `dotnet run` defaults to Production
# and ignores appsettings.Development.json — local runs need DOTNET_ENVIRONMENT.
migrate: ## Apply pending migrations (args: TENANT=<id> CATALOG_ONLY=1 SEED=1)
	DOTNET_ENVIRONMENT=Development dotnet run --project $(MIGRATOR) -- apply \
		$(if $(TENANT),--tenant $(TENANT)) \
		$(if $(CATALOG_ONLY),--catalog-only) \
		$(if $(SEED),--seed)

seed: ## Run SeedAsync only (args: TENANT=<id>)
	DOTNET_ENVIRONMENT=Development dotnet run --project $(MIGRATOR) -- seed $(if $(TENANT),--tenant $(TENANT))

seed-demo: ## Provision demo tenants (dev only: DOTNET_ENVIRONMENT=Development)
	DOTNET_ENVIRONMENT=Development dotnet run --project $(MIGRATOR) -- seed-demo

migrate-pending: ## Print pending migrations without applying
	DOTNET_ENVIRONMENT=Development dotnet run --project $(MIGRATOR) -- list-pending

migration: ## Add an EF migration (usage: make migration NAME=AddFoo MODULE=Catalog)
ifndef NAME
	$(error NAME is required, e.g. make migration NAME=AddFoo MODULE=Catalog)
endif
ifndef MODULE
	$(error MODULE is required, e.g. make migration NAME=AddFoo MODULE=Catalog)
endif
	dotnet tool restore
	dotnet ef migrations add $(NAME) \
		--project $(MIGRATIONS) \
		--startup-project $(API) \
		--context $(MODULE)DbContext

.PHONY: test test-unit test-integration test-module
test: ## Full backend suite (integration needs Docker)
	dotnet test $(SLNX)

test-unit: ## Unit suites incl. architecture tests (mirrors Backend CI)
	for proj in $(UNIT_TESTS); do \
		dotnet test "src/Tests/$${proj}.Tests" -c $(TEST_CONFIG) \
			--collect:"XPlat Code Coverage" --settings coverage.runsettings \
			--results-directory ./TestResults \
			--logger "trx;LogFileName=$${proj}.trx" || exit 1; \
	done

test-integration: ## Testcontainers suites (Docker must be running)
	dotnet test src/Tests/Integration.Tests -c $(TEST_CONFIG)
	dotnet test src/Tests/Integration.Middleware.Tests -c $(TEST_CONFIG)

test-module: ## One test project (usage: make test-module MODULE=Billing)
ifndef MODULE
	$(error MODULE is required, e.g. make test-module MODULE=Billing)
endif
	dotnet test "src/Tests/$(MODULE).Tests"

.PHONY: lint-ui build-ui test-e2e test-e2e-admin test-e2e-dashboard
lint-ui: ## ESLint both React apps
	cd clients/admin && npm run lint
	cd clients/dashboard && npm run lint

build-ui: ## Typecheck + production build both React apps
	cd clients/admin && npm run build
	cd clients/dashboard && npm run build

test-e2e-admin: ## Playwright suite, admin (route-mocked, no backend)
	cd clients/admin && npm run test:e2e

test-e2e-dashboard: ## Playwright suite, dashboard (route-mocked, no backend)
	cd clients/dashboard && npm run test:e2e

test-e2e: test-e2e-admin test-e2e-dashboard ## Both Playwright suites

.PHONY: verify clean doctor docker-up docker-down
verify: build lint-ui test-unit ## Local gate: build + lint + unit tests

clean: ## Clean build outputs and test results
	dotnet clean $(SLNX)
	rm -rf ./TestResults

doctor: tools ## Check environment (SDK, Docker, Aspire, ports)
	dotnet fsh doctor

docker-up: ## Production-like stack (Postgres, Valkey, MinIO, API, apps)
	cd deploy/docker && docker compose up --build

docker-down: ## Stop the docker stack
	cd deploy/docker && docker compose down
