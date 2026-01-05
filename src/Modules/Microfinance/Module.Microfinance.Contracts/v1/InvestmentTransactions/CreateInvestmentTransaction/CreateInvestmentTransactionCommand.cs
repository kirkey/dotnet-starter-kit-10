using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.InvestmentTransactions.CreateInvestmentTransaction;

public sealed record CreateInvestmentTransactionCommand(string Name) : ICommand<Guid>;
