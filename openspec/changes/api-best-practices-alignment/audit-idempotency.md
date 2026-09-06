# Idempotency triage (task 3.2, verified 2026-09-05)

## Key-optional behavior — CONFIRMED (code read)

`IdempotencyEndpointFilter.InvokeAsync` passes through when no `Idempotency-Key` header is present
(lines 40-44). Default header `Idempotency-Key`, TTL 24h, max length 128 (`IdempotencyOptions`).
Adding `.WithIdempotency()` is safe for clients that send no key.

## Endpoints given `.WithIdempotency()` in this change (4)

- `Chat/.../CreateChannel/CreateChannelEndpoint.cs` (creates channel row)
- `Catalog/.../Products/AddProductImage/AddProductImageEndpoint.cs` (creates image row)
- `Identity/.../Groups/CreateGroup/CreateGroupEndpoint.cs` (creates group row)
- `Multitenancy/.../RenewTenant/RenewTenantEndpoint.cs` (extends term + issues invoice)

Eight other create-endpoints audited already carry it (`CreateTicket`, `CreateProduct`,
`CreateBrand`, `CreateCategory`, `CreateWebhookSubscription`, `CreateTenant`, `AddTicketComment`,
`RequestUploadUrl`).

## Deliberately NOT marked (replay is pointless or harmful to promise)

- Auth/token/2FA flows, password flows, registration (conflict-guarded), session revocations,
  read-marking (`Mark*Read`), state-machine flips (`Void/Resolve/Close/Reopen/Approve/Reject`,
  restores, theme/validity sets), find-or-create (`FindOrCreateDm`), test sends
  (`TestWebhookSubscription`). Double-submit of these is either impossible, already
  idempotent by state, or surfaces an explicit conflict — never a silent duplicate.

## DEFECT — FIXED (was: pre-existing shared infra, replays never hit)

Root cause (proven by round-trip test): `HybridCache.SetAsync` namespaces its L2 keys
(`__MSFT_HCT__*`), so the filter's raw `IDistributedCache.GetAsync` probe could never see
HybridCache writes. Fix (approved): the filter now writes AND probes via `IDistributedCache`
directly with explicit JSON; response bytes are captured by single-execution tee of the
`IResult` so replays are byte-identical (previously would have replayed the result envelope).
Files/streams skip caching; entries expire by 24h TTL (nothing purged by tag).
Verified: live double-submit replays byte-identical with `Idempotency-Replayed: true`;
`ChatSendMessageTests` replay test un-skipped; new billing-plan replay test added.
