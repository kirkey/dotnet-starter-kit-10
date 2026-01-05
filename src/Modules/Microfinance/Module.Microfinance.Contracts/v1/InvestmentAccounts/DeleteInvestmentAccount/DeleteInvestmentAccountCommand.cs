using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.InvestmentAccounts.DeleteInvestmentAccount;

public sealed record DeleteInvestmentAccountCommand(Guid Id) : ICommand;
