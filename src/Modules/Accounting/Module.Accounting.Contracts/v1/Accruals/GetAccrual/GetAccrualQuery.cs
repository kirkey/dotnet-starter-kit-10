using FSH.Module.Accounting.Contracts.v1.Accruals;
using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.Accruals.GetAccrual;

public record GetAccrualQuery(Guid Id) : IQuery<AccrualDto>;