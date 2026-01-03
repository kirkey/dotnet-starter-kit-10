namespace FSH.Modules.Microfinance.Contracts.v1.SavingsAccounts;

public record GetSavingsAccountQuery(Guid Id);
public record GetSavingsAccountsQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive);
public record SavingsAccountsPagedResponse(List<SavingsAccountSummaryDto> Items, int TotalCount, int Page, int PageSize);
