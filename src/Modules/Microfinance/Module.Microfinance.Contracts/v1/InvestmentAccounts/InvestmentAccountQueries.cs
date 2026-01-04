namespace FSH.Module.Microfinance.Contracts.v1.InvestmentAccounts;

public record GetInvestmentAccountQuery(Guid Id);
public record GetInvestmentAccountsQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive);
public record InvestmentAccountsPagedResponse(List<InvestmentAccountSummaryDto> Items, int TotalCount, int Page, int PageSize);
