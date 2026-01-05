using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.TrialBalance.GenerateTrialBalance;

public sealed record GenerateTrialBalanceCommand(Guid Id) : ICommand;
