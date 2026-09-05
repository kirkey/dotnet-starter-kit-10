## Why

A sample audit of `src/Modules` against the conventions in `.agents/rules/` and `AGENTS.md` found real drift: 28 handlers sit on the `HandlerValidatorPairingTests` KnownMissing allowlists, ~45% of awaits lack `.ConfigureAwait(false)`, several request-path `catch (Exception)` blocks and bare `IgnoreQueryFilters()` calls need triage, and endpoint auth/idempotency coverage is unverified by tests. Fixing these now — while `Architecture.Tests` are green (55/55) — prevents silent handler, auth, and tenant-isolation regressions later.

## What Changes

- Audit every module's API slices (endpoints, handlers, validators, EF queries, exception handling, logging) against the repo conventions and record each gap with file and line.
- Remediate confirmed gaps to best-practice code: add the 28 missing validators (shrinking the allowlists), add missing `.ConfigureAwait(false)`, narrow or justify request-path `catch (Exception)`, verify explicit re-filters behind bare `IgnoreQueryFilters()`, gate or explicitly mark-anonymous every endpoint, add `.WithIdempotency()` to replay-safe POSTs.
- No API contract changes: same routes, status codes, and DTOs. Where a new validator would newly reject previously-accepted input, that is flagged as a contract impact and covered by tests — nothing breaks silently.
- No new runtime dependencies, no config changes, no migration changes.

## Capabilities

### New Capabilities

- `api-best-practices`: requirements for API-side convention compliance (validator pairing, async, exception handling, tenant-filter discipline, endpoint auth/idempotency, structured logging/CT propagation) plus the no-regression contract every remediation must satisfy.

### Modified Capabilities

(none — no existing specs; `openspec/specs/` is empty.)

## Impact

- Code: `src/Modules/*` (handlers, endpoints, validators, EF queries), `src/Tests/Architecture.Tests` (shrinking KnownMissing allowlists; optional new convention tests).
- Systems: none at runtime — behavior-preserving by contract; API, DB schema, and configs unchanged.
- Risk: validator additions can newly reject input (contract impact, handled per the spec); `ConfigureAwait`/logging/exception-shape edits are behavior-neutral.
