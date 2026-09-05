# `catch (Exception)` triage (task 2.2, verified 2026-09-05)

Verdict: all 26 sites compliant — KEEP, no edits. Buckets per design Decision 4:

## Keep: logged + rethrow (request path intact, mapping unchanged)

- `Auditing/.../AuditHttpMiddleware.cs:53` — writes exception audit, restores body, `throw;`
- `Multitenancy/.../Provisioning/TenantProvisioningJob.cs:83` — logs, marks step failed, `throw;`
- `Webhooks/.../Services/WebhookDispatchJob.cs:153` — records attempt, logs, rethrows for Hangfire retry (OCE/specific catches rethrown above it)

## Keep: logged + continue (background loops / per-item isolation, OCE excluded)

- `Billing/.../Services/BillingService.cs:151` — per-tenant loop, `#pragma CA1031` with reason, `LogError`
- `Multitenancy/.../Services/TenantExpiryScanJob.cs:71` — per-tenant loop, `#pragma CA1031`, `LogError`
- `Webhooks/.../Services/WebhookFanoutHandler.cs:91` — per-subscription, `when (ex is not OCE)`, `LogWarning`
- `Webhooks/.../Services/WebhookDeliveryService.cs:50` — records delivery failure, `when (ex is not OCE)`, `LogWarning`
- `Chat/.../Internal/ChatAttachmentUrls.cs:51` — per-attachment, `#pragma CA1031`, OCE excluded, documented fallback
- `Identity/.../Authorization/RolePermissionSyncHostedService.cs:50,78,104` — best-effort startup sync, all `when (ex is not OCE)`, logged
- `Identity/.../Services/SessionCleanupHostedService.cs:47` — OCE handled above, logged, loop continues
- `Multitenancy/.../GetTenantMigrations/GetTenantMigrationsQueryHandler.cs:68` — per-tenant failure recorded in response, others unaffected
- `Multitenancy/.../TenantMigrationsHealthCheck.cs:71` — reports-not-throws by design (readiness payload)
- `Files/.../Jobs/PurgeOrphanedFilesJob.cs:44`, `PurgeDeletedFilesJob.cs:46,63` — best-effort storage/quota steps, `LogWarning`
- `Notifications/.../IntegrationEventHandlers/BillingEmailSender.cs:27` — best-effort send, PII-safe `LogWarning`
- `Identity/.../Events/UserRegisteredEmailHandler.cs:44` — welcome mail must not break registration, PII-safe `LogWarning`
- `Auditing/.../Persistence/FileAuditDlqSink.cs:83` — last-resort sink, `LogError`, nowhere to escalate
- `Auditing/.../Core/AuditBackgroundWorker.cs:67,128,163` — logged, worker survives; OCE handled at outer scope
- `Identity/.../Impersonation/EndImpersonation/EndImpersonationCommandHandler.cs:83` — logged, actor swap proceeds by design
- `Identity/.../Tokens/TokenGeneration/GenerateTokenCommandHandler.cs:106` — non-critical session creation, `LogWarning`
- `Identity/.../Tokens/RefreshToken/RefreshTokenCommandHandler.cs:75` — parse fallback, `LogDebug`

No request-path catch converts exceptions into wrong statuses; the global ProblemDetails mapping is unchanged (verified: no edits made).
