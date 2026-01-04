using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.AccountingPeriods.CreateAccountingPeriod;


public record CreateAccountingPeriodCommand(string Name, string? Description) : ICommand<Guid>;