using FSH.Modules.Auditing.Contracts;
using FSH.Modules.Auditing.Contracts.Dtos;
using FSH.Modules.Auditing.Contracts.v1.GetAuditSummary;
using FSH.Modules.Auditing.Persistence;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Modules.Auditing.Features.v1.GetAuditSummary;

public sealed class GetAuditSummaryQueryHandler(AuditDbContext dbContext)
    : IQueryHandler<GetAuditSummaryQuery, AuditSummaryAggregateDto>
{
    public async ValueTask<AuditSummaryAggregateDto> Handle(GetAuditSummaryQuery query, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);

        IQueryable<AuditRecord> audits = dbContext.AuditRecords.AsNoTracking();

        if (query.FromUtc.HasValue)
        {
            audits = audits.Where(a => a.OccurredAtUtc >= query.FromUtc.Value);
        }

        if (query.ToUtc.HasValue)
        {
            audits = audits.Where(a => a.OccurredAtUtc <= query.ToUtc.Value);
        }

        if (!string.IsNullOrWhiteSpace(query.TenantId))
        {
            audits = audits.Where(a => a.TenantId == query.TenantId);
        }

        List<AuditRecord> list = await audits.ToListAsync(cancellationToken).ConfigureAwait(false);

        AuditSummaryAggregateDto aggregate = new();

        foreach (AuditRecord record in list)
        {
            AuditEventType type = (AuditEventType)record.EventType;
            aggregate.EventsByType[type] = aggregate.EventsByType.TryGetValue(type, out long c) ? c + 1 : 1;

            AuditSeverity severity = (AuditSeverity)record.Severity;
            aggregate.EventsBySeverity[severity] = aggregate.EventsBySeverity.TryGetValue(severity, out long s) ? s + 1 : 1;

            if (!string.IsNullOrWhiteSpace(record.Source))
            {
                string key = record.Source!;
                aggregate.EventsBySource[key] = aggregate.EventsBySource.TryGetValue(key, out long cs) ? cs + 1 : 1;
            }

            if (!string.IsNullOrWhiteSpace(record.TenantId))
            {
                string tenantKey = record.TenantId!;
                aggregate.EventsByTenant[tenantKey] = aggregate.EventsByTenant.TryGetValue(tenantKey, out long ct) ? ct + 1 : 1;
            }
        }

        return aggregate;
    }
}

