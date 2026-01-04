using FSH.Module.Auditing.Contracts.Dtos;
using Mediator;

namespace FSH.Module.Auditing.Contracts.v1.GetAuditById;

public sealed record GetAuditByIdQuery(Guid Id) : IQuery<AuditDetailDto>;

