using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.InvestmentAccounts.GetInvestmentAccounts;

public sealed record GetInvestmentAccountsQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive) : IQuery<InvestmentAccountsPagedResponse>;
