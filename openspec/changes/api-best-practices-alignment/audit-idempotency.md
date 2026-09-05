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

## DEFECT (pre-existing, shared infra): replays never hit

Live double-submit test against the running API (brand create ×2, ticket create ×2, same
`Idempotency-Key`): second request re-executed the handler both times (new GUIDs; brand case
returned the handler's own 409) with no `Idempotency-Replayed` header and no cache-write
warning in the log. Suspect: `HybridCache.SetAsync` write vs the manual
`IDistributedCache.GetAsync` + camelCase-`JsonSerializer.Deserialize` probe round-trip in
`src/BuildingBlocks/Web/Idempotency/IdempotencyEndpointFilter.cs` (protected code — not
touched). Affects all 33 idempotent endpoints, pre-existing, unrelated to the 4 additions.
Needs a BuildingBlocks-level investigation with explicit approval.
