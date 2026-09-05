## Purpose

This capability defines the API-side convention-compliance contract for the modular monolith: every endpoint, handler, validator, and query follows the documented best practices, and any remediation preserves existing API behavior.

## ADDED Requirements

### Requirement: Command and paginated-query handlers have validators

Every command handler and every paginated query handler SHALL have a corresponding `{Name}Validator`, enforced by `Architecture.Tests` (`HandlerValidatorPairingTests`) with empty KnownMissing allowlists.

#### Scenario: Allowlist is empty and tests pass
- **WHEN** `dotnet test src/Tests/Architecture.Tests` runs
- **THEN** `CommandHandlers_Should_Have_Corresponding_Validators` and the query-pairing test pass with zero entries in `KnownMissingCommandHandlers` and `KnownMissingQueryHandlers`

#### Scenario: Previously allowlisted handler gains a validator
- **WHEN** a handler from the 28-entry baseline (e.g. `VoidInvoiceCommandHandler`, `SearchProductsQueryHandler`) is remediated
- **THEN** its validator lives in the same feature folder, is auto-registered via `ModuleLoader`, and the handler's name is removed from the allowlist

### Requirement: Remediation preserves API contracts

Remediation SHALL NOT change routes, response status codes, or DTO shapes. A new validator that would newly reject previously-accepted input is a contract impact: it SHALL be flagged in the change notes and covered by handler tests proving the rejection (400 via `ValidationBehavior`) before merge.

#### Scenario: Behavior-neutral fix merges silently
- **WHEN** a fix only adds `.ConfigureAwait(false)`, structured logging, or an explicit re-filter
- **THEN** all existing unit and integration tests pass unmodified and no client-visible behavior changes

#### Scenario: New validator narrows accepted input
- **WHEN** a validator is added where none existed
- **THEN** the invalid-input case has a test asserting rejection, and the impact is recorded so API consumers are not surprised

### Requirement: Every endpoint is auth-gated or explicitly anonymous

Every Minimal API endpoint SHALL either carry `.RequirePermission(...)` or be recorded as anonymous-by-design (e.g. token issuance, email confirmation, webhook receivers) in the audit list. No endpoint is left unclassified.

#### Scenario: Full endpoint inventory is classified
- **WHEN** the audit enumerates all `Map{Post,Get,Put,Delete,Patch}` sites
- **THEN** each site maps to exactly one of `gated` or `anonymous-by-design with reason`, and any `gated`-missing site is fixed

#### Scenario: Anonymous login still works
- **WHEN** `POST /api/v1/identity/token/issue` is called without credentials context
- **THEN** it remains reachable anonymously and returns 200 with valid credentials

### Requirement: Replay-safe POSTs are idempotent

Every POST endpoint whose handler performs a non-idempotent write (create, charge, dispatch) SHALL carry `.WithIdempotency()`.

#### Scenario: Double-submitted create is safe
- **WHEN** the same idempotency-keyed create request is submitted twice
- **THEN** the second submission does not create a duplicate resource

### Requirement: Async continuations do not capture context

Every `await` in module request paths, background jobs, and hosted services SHALL use `.ConfigureAwait(false)`.

#### Scenario: Handler awaits are context-free
- **WHEN** the codebase is scanned for `await` expressions outside test and UI projects
- **THEN** each one chains `.ConfigureAwait(false)`

### Requirement: Exceptions map to correct ProblemDetails

Request-path code SHALL NOT catch broad `Exception` in ways that alter the status code mapping in `api-conventions.md` (framework types → RFC 9457). Background loops that catch to stay alive SHALL log with context and exclude `OperationCanceledException` via filtered catch.

#### Scenario: Known failure returns its documented status
- **WHEN** a request triggers `NotFoundException`, `ForbiddenException`, or `UnauthorizedException`
- **THEN** the response is the documented 404 / 403 / 401 ProblemDetails, unchanged by remediation

#### Scenario: Background job survives a failing item
- **WHEN** a background loop item throws a non-cancellation exception
- **THEN** the exception is logged with context, the loop continues, and cancellation still stops the host promptly

### Requirement: Tenant-filter bypasses are explicit and re-filtered

Any `IgnoreQueryFilters()` call that bypasses the tenant filter SHALL be followed by an explicit tenant re-filter. Bypass of only the SoftDelete filter SHALL use the named form `IgnoreQueryFilters([QueryFilters.SoftDelete])`.

#### Scenario: Cross-tenant read stays scoped
- **WHEN** code reads across tenants with filters ignored
- **THEN** the query explicitly constrains `TenantId`, and review confirms no unscoped read remains

#### Scenario: Trash views bypass only soft-delete
- **WHEN** a trash/restore query needs deleted rows
- **THEN** it uses the named SoftDelete-only bypass so tenant isolation stays enforced

### Requirement: Logging is structured and cancellable

Log statements SHALL use message templates (no string interpolation) and EF/IO calls SHALL propagate `CancellationToken`.

#### Scenario: Log audit is clean
- **WHEN** module sources are scanned for interpolated log messages
- **THEN** no matches are found and the build's analyzers stay green
