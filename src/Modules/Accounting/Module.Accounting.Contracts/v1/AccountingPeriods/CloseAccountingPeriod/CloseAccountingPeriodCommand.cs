using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.AccountingPeriods.CloseAccountingPeriod;


public record CloseAccountingPeriodCommand(Guid Id) : ICommand;