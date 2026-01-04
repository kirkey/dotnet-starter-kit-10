using FSH.Module.Auditing.Contracts;
using FSH.Module.Auditing.Contracts.Dtos;
using FSH.Module.Auditing.Contracts.v1.GetAuditById;
using FSH.Module.Auditing.Persistence;
using Mediator;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace FSH.Module.Auditing.Features.v1.GetAuditById;

public sealed class GetAuditByIdQueryHandler(AuditDbContext dbContext)
    : IQueryHandler<GetAuditByIdQuery, AuditDetailDto>
{
    public async ValueTask<AuditDetailDto> Handle(GetAuditByIdQuery query, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);

        AuditRecord? record = await dbContext.AuditRecords
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.Id == query.Id, cancellationToken)
            .ConfigureAwait(false);

        if (record is null)
        {
            throw new KeyNotFoundException($"Audit record {query.Id} not found.");
        }

        JsonElement payload;
        try
        {
            using JsonDocument document = JsonDocument.Parse(record.PayloadJson);
            payload = document.RootElement.Clone();
        }
        catch
        {
            payload = JsonDocument.Parse("{}").RootElement.Clone();
        }

        return new AuditDetailDto
        {
            Id = record.Id,
            OccurredAtUtc = record.OccurredAtUtc,
            ReceivedAtUtc = record.ReceivedAtUtc,
            EventType = (AuditEventType)record.EventType,
            Severity = (AuditSeverity)record.Severity,
            TenantId = record.TenantId,
            UserId = record.UserId,
            UserName = record.UserName,
            TraceId = record.TraceId,
            SpanId = record.SpanId,
            CorrelationId = record.CorrelationId,
            RequestId = record.RequestId,
            Source = record.Source,
            Tags = (AuditTag)record.Tags,
            Payload = payload
        };
    }
}

