namespace FSH.Modules.Microfinance.Contracts.v1.ShareAccounts;

public record GetShareAccountQuery(Guid Id);
public record GetShareAccountsQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive);
public record ShareAccountsPagedResponse(List<ShareAccountSummaryDto> Items, int TotalCount, int Page, int PageSize);
