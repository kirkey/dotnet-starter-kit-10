using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.InvestmentAccounts.UpdateInvestmentAccount;

public sealed record UpdateInvestmentAccountCommand(Guid Id, string Name) : ICommand<Guid>;
