# FullStackHero .NET Starter Kit

> Modular .NET 10 monolith (Vertical Slice) + two React 19 apps — multi-tenant SaaS.

This file is canonical for **all** AI tools. `CLAUDE.md` / `GEMINI.md` just import it — edit here, not there.
**Before touching an area, read its rule file** in the index below. Rules beat memory.

## Stack (non-obvious bits)

- Backend: Minimal APIs + **Mediator 3.x source-gen CQRS (not MediatR)** · FluentValidation · EF Core 10 / PostgreSQL · Finbuckle multitenancy · HybridCache on Valkey · Hangfire · OpenAPI + Scalar · Serilog + OpenTelemetry.
- Frontend (`clients/admin` operator, `clients/dashboard` tenant): React 19 + Vite 7 + TS · TanStack Query v5 · React Router 7 · Radix + Tailwind v4 · SignalR/SSE. **Runtime `/config.json`, not `VITE_*`** (`VITE_API_BASE_URL` is dev-proxy target only). API client is **hand-written `apiFetch`, no codegen**. Playwright suites are **route-mocked, no backend**.

## Repo map

| Path | What |
|------|------|
| `src/BuildingBlocks/` | Shared framework. **Protected — approval required to touch.** |
| `src/Modules/{Name}/` | Bounded context = runtime project + `.Contracts` project (its only public API). |
| `src/Host/FSH.Starter.Api` | Composition root. DB is **not** migrated at startup. |
| `src/Host/FSH.Starter.AppHost` | Aspire orchestrator: Postgres, Valkey, MinIO, migrator, API, both React apps. |
| `src/Host/FSH.Starter.DbMigrator` | One-shot migrate/seed runner (verbs below). |
| `src/Host/FSH.Starter.Migrations.PostgreSQL` | All migrations, per-module folders, per-module snapshots. |
| `src/Tests/` | `{Module}.Tests` units · `Architecture.Tests` (NetArchTest) · `Integration.Tests` (Testcontainers, needs Docker). |
| `src/Tools/CLI` · `clients/*` · `deploy/` | `fsh` CLI · the two React apps · docker + terraform. |

## Build, run, migrate

`make help` lists the shortcuts (`run-api`, `run-ui`, `migrate`, `test-module MODULE=X`, …) — they wrap the raw commands below.

```bash
dotnet run --project src/Host/FSH.Starter.AppHost   # whole stack (one-time: npm install in both clients/)
dotnet build src/FSH.Starter.slnx
dotnet run --project src/Host/FSH.Starter.Api       # https://localhost:7030 (/scalar)
dotnet test src/FSH.Starter.slnx                    # integration suites REQUIRE Docker
dotnet test src/Tests/{Module}.Tests                # one project; unit projects need no Docker
cd clients/{app} && npm run dev                     # admin :5173, dashboard :5174
cd clients/{app} && npm run test:e2e                # Playwright, route-mocked
```

```bash
DOTNET_ENVIRONMENT=Development dotnet run --project src/Host/FSH.Starter.DbMigrator -- [apply|seed|seed-demo|list-pending] [--seed] [--tenant <id>] [--catalog-only]
# Default verb is apply. seed-demo is dev-only (DOTNET_ENVIRONMENT=Development).
# The env prefix matters: DbMigrator has no launchSettings, so without it the run
# is Production and appsettings.Development.json is silently ignored.
```

**Ports:** API 7030/5030 · admin 5173 · dashboard 5174 · Aspire 15888 · Postgres 5432 · pgAdmin 5050 · Valkey 6379 · MinIO 9000/9001.

## Branches & CI

Default branch is **`develop`** — branch from and target it. (Upstream uses `main`; this fork diverged.)
⚠️ `backend.yml` / `frontend.yml` still trigger on `main` only, so pushes and PRs to `develop` run **no CI** — missing checks mean "not configured", not "passing". Gates are path-scoped (`Backend CI` on `src/**`, `Frontend CI` on `clients/**`) and report green when only the other side changed.

## Golden rules (do not break)

1. **Module boundaries** — a module references another module only through its `.Contracts` project, never its runtime project. Enforced by `Architecture.Tests`.
2. **Registering a module touches FOUR places** — `Program.cs` Mediator `o.Assemblies` (two markers each) + `moduleAssemblies` array, **and the identical pair in `DbMigrator/Program.cs`**. A missing Mediator marker = handlers silently undiscovered. See `architecture.md`.
3. **Tenant isolation is default-ON** via `BaseDbContext`. Opt out only via `IGlobalEntity`. Subclass DbContexts call `base.OnModelCreating` **last**. See `database.md`.
4. **Do NOT modify `src/BuildingBlocks`** without explicit approval — shared by every module, wide blast radius.
5. **Mediator handlers must be `public sealed`**, return `ValueTask<T>`, and `.ConfigureAwait(false)` every await.
6. **Structured logging only** — no string interpolation in log messages; use message templates / `[LoggerMessage]`.
7. **Propagate `CancellationToken`** into every EF/IO call; add as `= default` on public service methods.
8. **Every command handler + paginated query handler needs a validator** (`{Name}Validator`). Enforced by `Architecture.Tests`.
9. **Frontend: pass per-call data through `mutate(arg)`**, never via state the mutation callbacks close over (execute-time race). See `frontend/shared.md`.

## Rules index — read the relevant file before you work

**Backend / cross-cutting** (`.agents/rules/`)

| Working on… | Read |
|---|---|
| Module structure, boundaries, registration, DI, middleware order, config | `architecture.md` |
| Endpoints, CQRS, validation, exceptions, permissions, versioning | `api-conventions.md` |
| EF Core, entities, migrations, tenant isolation, query filters | `database.md` |
| Cross-module events, Outbox/Inbox, idempotent handlers | `eventing.md` |
| Caching (HybridCache/Redis), keys, invalidation | `caching.md` |
| Background jobs (Hangfire), recurring jobs | `jobs.md` |
| Outbound HTTP resilience (Polly) | `resilience.md` |
| Files/blobs, presigned uploads, providers | `storage.md` |
| CORS, security headers, rate limiting, idempotency, quotas | `security.md` |
| SignalR / SSE backend | `realtime.md` |
| Logging, correlation, OpenTelemetry | `logging.md` |
| Unit test conventions, NetArchTest | `testing.md` |
| Integration tests (Testcontainers harness + gotchas) | `integration-testing.md` |
| **Modifying `src/BuildingBlocks`** (read first — it's protected) | `buildingblocks-protection.md` |
| A specific module's quirks | `modules/{module}.md` (identity, multitenancy, chat, files, webhooks, auditing, billing, catalog, tickets, notifications) |

**Frontend** (`.agents/rules/frontend/`)

| Working on… | Read |
|---|---|
| Any React work (shared stack, API client, Query, Tailwind, design language) | `frontend/shared.md` |
| The operator app (`clients/admin`) | `frontend/admin.md` |
| The tenant app (`clients/dashboard`) | `frontend/dashboard.md` |

## Coding style (backend)

File-scoped namespaces · 4-space indent · explicit types (`var` only when RHS-obvious) · `is null` /
`is not null` · pattern matching + switch expressions · `ArgumentNullException.ThrowIfNull` guards ·
records for DTOs/events/value objects · `default!` for required non-nullable strings.
Warnings fail the build (`TreatWarningsAsErrors` + `AllEnabledByDefault` analyzers in `src/Directory.Build.props`).

## Bulk edits

- **NEVER use scripts to touch code — no Python, no sed/awk, no throwaway rewrite scripts, in any case.** A past run corrupted hundreds of files at once (double `.ConfigureAwait(false)`, CA glued onto `?? throw` targets, CA after `is not null` checks) — text scanners can't see real syntax, and the damage compiles nowhere near the edit. The same ban covers `python3 -c` one-liners and script-generated edit lists: if a dedicated tool (Read/Edit/Grep/Glob) or the compiler can do it, use that instead.
- Make surgical file-level edits: Read the file, edit exactly, keep diffs reviewable, verify with `dotnet build` + tests.
- One mechanical pattern at a time; re-scan after each pass.

## Adding things (quick pointers)

- **Feature** — Contracts command/query → handler → validator → endpoint → wire in module `MapEndpoints()` → tests. Details: `api-conventions.md`.
- **Module** — new `Modules.{Name}` + `.Contracts`, implement `IModule` w/ assembly-level `[assembly: FshModule(typeof(XModule), order)]`, register in **all four places**, add migration folder + tests. Details: `architecture.md`.
- **Migration** — `dotnet tool restore` first (`dotnet-ef` is pinned in `.config/dotnet-tools.json`); full build before `migrations remove` or the snapshot eats the previous migration. Details: `database.md`.
- **React page** — API module (`src/api/`) → page (named export) → lazy route under `AppShell` → (admin) mirror permission + RouteGuard → Playwright test. Details: `frontend/shared.md`.

## Frontend API quirks

- Search params use **PascalCase** keys (`PageNumber`, `PageSize`); tenant header is lowercase `tenant`; login posts to `/api/v1/identity/token/issue` with header `X-FSH-App: "admin"|"dashboard"`.
- E2E helpers: `seedAuthedSession(page, TEST_USER)` then `installShellMocks(page)` in `beforeEach`; page-specific route mocks after shell mocks (most-recently-registered wins).

## AI tooling resources

- **Skills** (`.agents/skills/*/SKILL.md`) — read the skill before the task. FSH scaffolders: `add-feature`, `add-entity`, `add-module`, `add-react-page`, `add-full-slice`. Ops: `create-migration`, `add-permission`, `add-integration-event`. Reference: `query-patterns`, `testing-guide`, `mediator-reference`. Plus `openspec-*` (spec lifecycle) and `caveman-*` (compression/review helpers).
- **Workflows** (`.agents/workflows/*.md`) — task playbooks, including `opsx-*` for the OpenSpec lifecycle.
- **OpenSpec** — specs in `openspec/specs/`, proposals in `openspec/changes/` (`schema: spec-driven`); start work with the `openspec-propose` skill.
- **CodeGraph** — this repo is indexed (`.codegraph/`). Prefer `codegraph_explore` (MCP) or `codegraph explore "<question>"` (shell) over grep/find for locating code; it follows call paths grep can't.
- `opencode.jsonc` wires the superpowers plugin + CodeGraph MCP — don't remove either.
