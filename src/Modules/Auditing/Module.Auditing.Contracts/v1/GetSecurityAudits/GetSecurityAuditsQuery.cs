using FSH.Module.Auditing.Contracts;
using FSH.Module.Auditing.Contracts.Dtos;
using Mediator;

namespace FSH.Module.Auditing.Contracts.v1.GetSecurityAudits;

public sealed class GetSecurityAuditsQuery : IQuery<IReadOnlyList<AuditSummaryDto>>
{
    public SecurityAction? Action { get; init; }

    public string? UserId { get; init; }

    public string? TenantId { get; init; }

    public DateTime? FromUtc { get; init; }

    public DateTime? ToUtc { get; init; }
}

