namespace FSH.Module.Microfinance.Contracts.v1.ShareTransactions;

public record GetShareTransactionQuery(Guid Id);
public record GetShareTransactionsQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive);
public record ShareTransactionsPagedResponse(List<ShareTransactionSummaryDto> Items, int TotalCount, int Page, int PageSize);
