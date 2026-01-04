using FSH.Module.Auditing.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace FSH.Module.Auditing.Persistence;

/// <summary>
/// Captures EF Core entity changes at SaveChanges to produce an EntityChange event.
/// </summary>
public sealed class AuditingSaveChangesInterceptor(IAuditPublisher publisher) : SaveChangesInterceptor
{
    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(eventData);
        DbContext? ctx = eventData.Context;
        if (ctx is null) return result;

        EntityEntry[] entries = ctx.ChangeTracker.Entries()
            .Where(e => e.State is EntityState.Added or EntityState.Modified or EntityState.Deleted)
            .ToArray();

        if (entries.Length == 0) return result;

        List<EntityDiffBuilder.Diff> diffs = EntityDiffBuilder.Build(entries);

        if (diffs.Count > 0)
        {
            foreach (IGrouping<(string DbContext, string? Schema, string Table, string EntityName, string Key, EntityOperation Operation), EntityDiffBuilder.Diff> group in diffs.GroupBy(d => (d.DbContext, d.Schema, d.Table, d.EntityName, d.Key, d.Operation)))
            {
                EntityChangeEventPayload payload = new(
                    DbContext: group.Key.DbContext,
                    Schema: group.Key.Schema,
                    Table: group.Key.Table,
                    EntityName: group.Key.EntityName,
                    Key: group.Key.Key,
                    Operation: group.Key.Operation,
                    Changes: group.SelectMany(g => g.Changes).ToList(),
                    TransactionId: ctx.Database.CurrentTransaction?.TransactionId.ToString());

                AuditEnvelope env = new(
                    id: Guid.CreateVersion7(),
                    occurredAtUtc: DateTime.UtcNow,
                    receivedAtUtc: DateTime.UtcNow,
                    eventType: AuditEventType.EntityChange,
                    severity: AuditSeverity.Information,
                    tenantId: null, userId: null, userName: null,
                    traceId: null, spanId: null, correlationId: null, requestId: null,
                    source: ctx.GetType().Name,
                    tags: AuditTag.None,
                    payload: payload);

                await publisher.PublishAsync(env, cancellationToken);
            }
        }

        return result;
    }
}
