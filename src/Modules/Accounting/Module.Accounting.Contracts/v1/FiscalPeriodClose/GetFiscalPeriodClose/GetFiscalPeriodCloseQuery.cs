using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.FiscalPeriodClose.GetFiscalPeriodClose;

public sealed record GetFiscalPeriodCloseByIdQuery(Guid Id) : IQuery<FiscalPeriodCloseDto>;