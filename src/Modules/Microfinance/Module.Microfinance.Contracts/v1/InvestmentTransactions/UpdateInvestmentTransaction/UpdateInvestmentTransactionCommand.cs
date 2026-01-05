using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.InvestmentTransactions.UpdateInvestmentTransaction;

public sealed record UpdateInvestmentTransactionCommand(Guid Id, string Name) : ICommand<Guid>;
