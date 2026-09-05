# Contract impacts of new validators (task 4.4, verified 2026-09-05)

Rule: previously-accepted input that a new validator now rejects is a contract impact.
All impacts below turn a silent handler clamp / downstream 404 into an explicit 400 via
`ValidationBehavior`. Invalid-input tests exist for every validator (see
`{Billing,Catalog,Identity,Multitenancy,Generic}.Tests/Validators/`); they assert at the
validator level per repo convention, while pipeline execution (`ValidationBehavior<,>`
via `ModuleLoader` auto-registration) is framework-owned behavior covered by the
framework's own suites.

## Empty-Guid rejections (were: downstream NotFoundException → 404; now: 400)

- `VoidInvoice`, `MarkInvoicePaid`, `IssueInvoice` (`InvoiceId`)
- `Delete/RestoreProduct`, `Delete/RestoreCategory`, `Delete/RestoreBrand`
- `Restore/Resolve/Reopen/AssignTicket` (`TicketId`)
- Risk: negligible — empty GUIDs are never legitimate; direct-mediator callers get a
  clearer error sooner. HTTP callers are unaffected in practice (route-constrained GUIDs).

## Paging rejections (were: handler clamp to defaults; now: 400)

- Billing invoice queries (`PageSize` 1..100, `PageNumber` > 0) — endpoint already clamps,
  so HTTP traffic is unaffected; direct sends outside the clamp window now fail fast.
- Catalog + Tickets + `GetTenantSessions` queries (`PageSize` 1..200, `PageNumber` > 0) —
  same story; bounds mirror each handler's clamp window (verified per handler).
- `PeriodMonth` 1..12 (Billing invoice queries) — month 13+ was never meaningful.

## New free-text bound (was: accepted; now: 400 over the cap)

- `VoidInvoice.Reason` > 512 chars — mirrors the sibling `CreateTopupRequest.Note` cap.

## Empty tenant rejection (was: downstream failure; now: 400)

- `RetryTenantProvisioning.TenantId` empty — previously failed later in provisioning.

## Authentication closure (was: open; now: 401 for anonymous)

- `GET /api/v1/billing/invoices/me` had no auth of any kind and now requires
  `BillingPermissions.View` (basic — every signed-in user keeps access). Unauthenticated
  callers go from 200 to 401. This closes an access-control hole, not a contract
  clients could rely on; recorded here so the status-code delta is explicit.
- `GET /api/v1/identity/permissions` gained explicit `.RequireAuthorization()`;
  behavior identical (the fallback policy already returned 401) — no impact.

## No behavior change

- `EnrollTwoFactor`, `EndImpersonation`, `ResetTenantTheme` validators are intentionally
  empty (parameterless commands) — pairing contract only.
- All `.ConfigureAwait(false)`, catch-triage (no edits), filter, auth-gate, and
  idempotency-attribute changes are behavior-neutral by construction.
