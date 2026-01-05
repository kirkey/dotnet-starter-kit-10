using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.InvestmentTransactions.GetInvestmentTransaction;

public sealed record GetInvestmentTransactionQuery(Guid Id) : IQuery<InvestmentTransactionDto>;
