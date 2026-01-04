using Finbuckle.MultiTenant;
using Finbuckle.MultiTenant.Abstractions;
using FSH.Framework.Shared.Multitenancy;
using FSH.Module.Auditing.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Module.Auditing.Persistence;

/// <summary>
/// Persists audit envelopes into SQL using EF Core.
/// </summary>
public sealed class SqlAuditSink(
    IServiceScopeFactory scopeFactory,
    IAuditSerializer serializer,
    ILogger<SqlAuditSink> log)
    : IAuditSink
{
    public async Task WriteAsync(IReadOnlyList<AuditEnvelope> batch, CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(batch);
        if (batch.Count == 0) return;

        // Process per-tenant so MultiTenantDbContext has an ambient tenant context.
        foreach (IGrouping<string?, AuditEnvelope> group in batch.GroupBy(e => e.TenantId))
        {
            using IServiceScope scope = scopeFactory.CreateScope();
            IMultiTenantStore<AppTenantInfo> store = scope.ServiceProvider.GetRequiredService<IMultiTenantStore<AppTenantInfo>>();

            AppTenantInfo? tenantInfo = group.Key is null
                ? await store.GetAsync(MultitenancyConstants.Root.Id).ConfigureAwait(false)
                : await store.GetAsync(group.Key).ConfigureAwait(false);

            if (tenantInfo is null)
            {
                log.LogWarning("Skipping audit write for tenant {TenantId} because tenant was not found.", group.Key ?? "<null>");
                continue;
            }

            scope.ServiceProvider.GetRequiredService<IMultiTenantContextSetter>()
                .MultiTenantContext = new MultiTenantContext<AppTenantInfo>(tenantInfo);

            AuditDbContext db = scope.ServiceProvider.GetRequiredService<AuditDbContext>();

            List<AuditRecord> records = group.Select(e => new AuditRecord
            {
                Id = e.Id,
                OccurredAtUtc = e.OccurredAtUtc,
                ReceivedAtUtc = e.ReceivedAtUtc,
                EventType = (int)e.EventType,
                Severity = (byte)e.Severity,
                TenantId = e.TenantId,
                UserId = e.UserId,
                UserName = e.UserName,
                TraceId = e.TraceId,
                SpanId = e.SpanId,
                CorrelationId = e.CorrelationId,
                RequestId = e.RequestId,
                Source = e.Source,
                Tags = (long)e.Tags,
                PayloadJson = serializer.SerializePayload(e.Payload)
            }).ToList();

            db.AuditRecords.AddRange(records);
            await db.SaveChangesAsync(ct).ConfigureAwait(false);

            log.LogInformation("Wrote {Count} audit records for tenant {TenantId}.", records.Count, tenantInfo.Id);
        }
    }
}
