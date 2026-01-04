using FSH.Module.Auditing.Contracts.Dtos;
using Mediator;

namespace FSH.Module.Auditing.Contracts.v1.GetAuditsByCorrelation;

public sealed class GetAuditsByCorrelationQuery : IQuery<IReadOnlyList<AuditSummaryDto>>
{
    public string CorrelationId { get; init; } = default!;

    public DateTime? FromUtc { get; init; }

    public DateTime? ToUtc { get; init; }
}

