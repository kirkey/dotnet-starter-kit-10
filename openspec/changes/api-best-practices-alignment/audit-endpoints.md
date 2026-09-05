# Endpoint auth inventory (task 1.2, verified 2026-09-05)

Scope: `*Endpoint.cs` files under `src/Modules` containing `Map(Post|Get|Put|Delete|Patch)`.
Module `*Module.cs` files and `NoAuditAttribute.cs` matched only in comments — excluded (not endpoints).

## Anonymous by design (6) — keep, record in test allowlist

- `Identity/.../Tokens/TokenGeneration/GenerateTokenEndpoint.cs` — login must be reachable pre-auth
- `Identity/.../Tokens/RefreshToken/RefreshTokenEndpoint.cs` — refresh by definition
- `Identity/.../Users/ConfirmEmail/ConfirmEmailEndpoint.cs` — email link flow
- `Identity/.../Users/ForgotPassword/ForgotPasswordEndpoint.cs` — password-reset initiation
- `Identity/.../Users/ResetPassword/ResetPasswordEndpoint.cs` — password-reset completion
- `Identity/.../Users/SelfRegistration/SelfRegisterUserEndpoint.cs` — public signup

## Authenticated, self-service by design (9) — keep (no fine-grained permission fits)

Subsequent sweep also caught `GetUserPermissions` (ungated, relying on fallback policy) — fixed
with explicit `.RequireAuthorization()` in 3.3 plus the `Endpoints_Should_Be_Gated_Or_Allowlisted`
architecture test locking the full inventory.

- `Identity/.../Impersonation/EndImpersonation/EndImpersonationEndpoint.cs` — `[Authorize]`; any impersonation session must be able to end (a permission the impersonated user may lack would lock them in)
- `Identity/.../TwoFactor/Enroll|Disable|VerifyEnroll` (3) — `RequireAuthorization`; every signed-in user manages their own 2FA
- `Identity/.../Users/ChangePassword`, `GetUserProfile` (throws `UnauthorizedException` when claim missing), `SetProfileImage`, `UpdateUser` (forces `request.Id = userId`) — `RequireAuthorization`; self-only by construction
- `Multitenancy/.../GetMyTenantStatus/GetMyTenantStatusEndpoint.cs` — `RequireAuthorization`; tenant-scoped self read (returns 401 without tenant context)

## Authenticated + ownership-enforced in handler (3) — VERIFIED in 3.1, keep as-is

- `Files/.../FinalizeUpload/FinalizeUploadEndpoint.cs` — `RequireAuthorization`; handler throws `ForbiddenException("not your pending file")` on `CreatedByUserId` mismatch
- `Files/.../GetFileDownloadUrl/GetFileDownloadUrlEndpoint.cs` — `RequireAuthorization`; handler resolves `IFileAccessPolicy` and throws 404 on policy denial (no existence leak)
- `Files/.../GetFileMetadata/GetFileMetadataEndpoint.cs` — `RequireAuthorization`; same policy gate as download URL

## MISSING gate (1) — FIXED in 3.1

- `Billing/.../Invoices/GetMyInvoices/GetMyInvoicesEndpoint.cs` — had no auth of any kind. Fixed with `.RequirePermission(BillingPermissions.View)` (mirrors sibling `GetMyWallet`/`GetMyTopupRequests`) plus `.ConfigureAwait(false)`.

## Gated (rest)

All other endpoint files carry `.RequirePermission(...)`. (`Multitenancy/.../GetTenantTheme` confirmed gated with `MultitenancyPermissions.Tenants.ViewTheme`.)
