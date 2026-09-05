# `IgnoreQueryFilters` triage (task 2.3, verified 2026-09-05)

## Named SoftDelete-only bypasses — compliant by construction (tenant filter stays)

Catalog `ListTrashed{Products,Categories,Brands}` + `Restore{Product,Category,Brand}`, Tickets `ListTrashedTickets` + `RestoreTicket`, `CreateTicket` count query. No action.

## Bare bypasses with explicit tenant re-filter — compliant

- `Auditing/.../GetAudits/GetAuditsQueryHandler.cs` + `GetAuditSummaryQueryHandler.cs` — `.Where(a => a.TenantId == requested)` after bypass, gated by `ViewCrossTenant`.
- `Identity/.../Services/IdentityService.cs` (`BuildClaimsForUserAsync`) — `EF.Property<string>(u, "TenantId") == tenantId` after bypass.
- `Identity/.../Services/UserCountQuotaGaugeProvider.cs` — `TenantId == tenantId` after bypass.
- `Identity/.../Services/IdentityService.cs` (`StoreRefreshTokenAsync`) — targeted `ExecuteUpdate` on globally-unique user Id; documented in code.

## Bare bypasses on entities with no tenant filter — compliant (nothing to drop)

- `FileAsset` carries **no TenantId column** (ownership via `OwnerType`/`OwnerId`/`CreatedByUserId`); `ChatChannel` likewise. Bare `IgnoreQueryFilters()` on these drops only the SoftDelete filter.
- Covered sites: Files `ListTrashedFiles`, `RestoreFile`, `PurgeOrphanedFilesJob`, `PurgeDeletedFilesJob`; Chat `RestoreChannel` (additionally gated by `ManageAll`).
- Access control lives in handler ownership/policy checks (verified: `FinalizeUpload` 403 on `CreatedByUserId` mismatch, metadata/download `IFileAccessPolicy` checks) and non-basic permission gates (`ViewTrash`, `Restore`).
- One comment fix: `ListTrashedFilesQueryHandler` claimed "tenant scoping via per-tenant DbContext" — corrected to state the entity has no tenant filter and scoping comes from the permission gate.

No behavior changes; `Integration.Tests` re-verifies in task 5.1.
