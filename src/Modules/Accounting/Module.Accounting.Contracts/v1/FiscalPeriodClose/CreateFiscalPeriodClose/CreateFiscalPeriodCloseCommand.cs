using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.FiscalPeriodClose.CreateFiscalPeriodClose;

public sealed record CreateFiscalPeriodCloseCommand(
    Guid FiscalPeriodId,
    int FiscalYear,
    string PeriodName,
    DateTime StartDate,
    DateTime EndDate,
    decimal RetainedEarnings = 0,
    string? Description = null) : ICommand<Guid>;