namespace FSH.Module.Microfinance.Contracts.v1.MobileTransactions;


public record MobileTransactionsPagedResponse(List<MobileTransactionSummaryDto> Items, int TotalCount, int Page, int PageSize);
