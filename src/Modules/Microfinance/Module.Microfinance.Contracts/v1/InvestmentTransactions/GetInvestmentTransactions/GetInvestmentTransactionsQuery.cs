using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.InvestmentTransactions.GetInvestmentTransactions;

public sealed record GetInvestmentTransactionsQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive) : IQuery<InvestmentTransactionsPagedResponse>;
