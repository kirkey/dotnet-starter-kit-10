using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.InvestmentAccounts.CreateInvestmentAccount;

public sealed record CreateInvestmentAccountCommand(string Name) : ICommand<Guid>;
