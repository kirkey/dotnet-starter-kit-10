namespace FSH.Module.Microfinance.Contracts.v1.InvestmentAccounts;

public record InvestmentAccountsPagedResponse(List<InvestmentAccountSummaryDto> Items, int TotalCount, int Page, int PageSize);
