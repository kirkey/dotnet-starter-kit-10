using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.AccountingPeriods.DeleteAccountingPeriod;


public record DeleteAccountingPeriodCommand(Guid Id) : ICommand;