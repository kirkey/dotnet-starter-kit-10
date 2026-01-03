namespace FSH.Modules.Microfinance.Contracts.v1.MobileTransactions;

public record GetMobileTransactionQuery(Guid Id);
public record GetMobileTransactionsQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive);
public record MobileTransactionsPagedResponse(List<MobileTransactionSummaryDto> Items, int TotalCount, int Page, int PageSize);
