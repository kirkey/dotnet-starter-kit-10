using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.AccountingPeriods.UpdateAccountingPeriod;


public record UpdateAccountingPeriodCommand(Guid Id, string Name, string? Description) : ICommand<Guid>;