namespace FSH.Module.Microfinance.Contracts.v1.ShareAccounts;

public record ShareAccountsPagedResponse(List<ShareAccountSummaryDto> Items, int TotalCount, int Page, int PageSize);
