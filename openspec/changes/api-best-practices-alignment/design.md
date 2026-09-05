## Context

See `proposal.md` (Why) for motivation. Current state (verified 2026-09-05, read-only audit):
- `Architecture.Tests`: 55/55 green. Shape-level conventions are enforced (endpoint static/shape/naming, versioning, module boundaries, tenant-entity rules, permission-attribute uniqueness) — but NOT per-endpoint auth gating, idempotency, `ConfigureAwait`, or catch discipline.
- `HandlerValidatorPairingTests` carries 28 KnownMissing entries (17 command + 11 query handlers, enumerated in the audit) — the largest concrete gap list.
- `src/Modules`: 1019 `await`s vs 567 `.ConfigureAwait(false)`; 26 `catch (Exception)` sites (token-handler ones verified compliant: logged, non-critical); ~20 `IgnoreQueryFilters()` sites (mostly named-form or commented; bare ones need re-filter proof); 177 `Map*` sites vs 159 `RequirePermission` occurrences; 68 POSTs vs 29 `.WithIdempotency()`.
- No interpolated logging found; all sampled handlers are `public sealed`.

## Goals / Non-Goals

**Goals:**
- Close every gap class with a repeatable detect → fix → verify loop, per class independently reviewable.
- Leave machine-checked locks behind where cheap (shrink allowlists to zero; add a gated-or-allowlisted endpoint test mirroring the KnownMissing pattern).
- Produce a classified endpoint inventory (gated vs anonymous-by-design) as the audit record.

**Non-Goals:**
- No contract changes (routes, status codes, DTOs), no migrations, no config changes, no new packages.
- No frontend, BuildingBlocks, or infrastructure changes.
- No new spec capabilities beyond `api-best-practices`.

## Decisions

1. **Roll out per gap class, not per module.** Each class (validators, ConfigureAwait, catches, filters, auth, idempotency) gets its own detect-fix-verify pass and its own commit(s). Alternative (module-by-module sweeps) mixes concerns and makes review harder; gap classes cut across all 10 modules uniformly.
   - Ordering: behavior-neutral classes first (ConfigureAwait, logging/CT, catch narrowing that preserves mapping, named-filter conversions), validator additions last since only they can alter accepted input.
2. **Evidence before edits.** Each pass starts by regenerating the inventory with the recorded `rg` queries and diffing against the baseline counts in this design, so new drift since planning is caught.
3. **Validators derived from command shape.** Each missing validator is written from the command/query's properties following sibling validators in the same module (same folder, `{Name}Validator`, auto-registered). Every added validator gets an invalid-input handler test; any newly-rejected input is flagged as a contract impact for review.
4. **Catch triage in three buckets.** (a) Compliant — logged with context, non-critical continuation, OCE excluded: keep, no touch. (b) Over-broad in request path — narrow to the specific exception types or remove and let the global ProblemDetails handler map. (c) Background loops — add missing OCE exclusion / context logging only.
5. **Filter triage per call site.** Bare `IgnoreQueryFilters()` must show an explicit tenant re-filter or convert to `IgnoreQueryFilters([QueryFilters.SoftDelete])` where only trash visibility is needed. Each site gets a review note, not a blind rewrite.
6. **Lock the endpoint classification with a test.** Add `Endpoints_Should_Be_Gated_Or_Allowlisted` to `Architecture.Tests` (same allowlist pattern as validators): every endpoint either has permission metadata or sits on an `AnonymousByDesign` list with a reason. This is the only new test file content; everything else reuses existing suites.
7. **Audit record lives in the change, not the repo.** The classified endpoint/filter inventories are working artifacts under `openspec/changes/api-best-practices-alignment/`; only code + tests merge.

## Risks / Trade-offs

- [New validator rejects traffic clients rely on] → contract-impact flag + invalid-input test + reviewer sign-off per validator; validators merge last, module by module.
- [Mass ConfigureAwait edits create review noise] → mechanical commits separated by gap class; verification is `rg` zero-count plus the full unit suite, no behavior to review line-by-line.
- [Wrong tenant re-filter breaks isolation or over-restricts] → each filter site reviewed individually; integration suites (`Integration.Tests`, Docker) must pass before merge.
- [`WithIdempotency` on a POST whose clients don't send keys] → check the idempotency mechanism's key-optional behavior first; only mark truly replay-safe endpoints.
- [Audit counts drifted since planning] → each pass re-runs the inventory queries; counts here are baselines, not targets set in stone.

## Migration Plan

No deployment migration: no schema, config, or contract changes. Rollout is a commit series (behavior-neutral first, validators last); rollback is revert of the corresponding commit(s). `Architecture.Tests` (55 baseline + new gating test) plus module unit suites gate every step; integration suites gate the filter and validator steps.

## Open Questions

- None that change specs, approach, or tasks. Per-endpoint anonymous-by-design reasons are recorded during the audit pass, not decided here.
