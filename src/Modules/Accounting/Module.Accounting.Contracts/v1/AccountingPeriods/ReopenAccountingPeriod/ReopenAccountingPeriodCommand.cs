using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.AccountingPeriods.ReopenAccountingPeriod;


public record ReopenAccountingPeriodCommand(Guid Id) : ICommand;