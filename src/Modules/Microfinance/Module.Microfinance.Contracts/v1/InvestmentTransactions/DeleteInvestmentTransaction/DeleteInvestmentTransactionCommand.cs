using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.InvestmentTransactions.DeleteInvestmentTransaction;

public sealed record DeleteInvestmentTransactionCommand(Guid Id) : ICommand;
