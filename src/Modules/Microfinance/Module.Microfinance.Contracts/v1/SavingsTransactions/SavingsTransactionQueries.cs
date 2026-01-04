namespace FSH.Module.Microfinance.Contracts.v1.SavingsTransactions;

public record GetSavingsTransactionQuery(Guid Id);
public record GetSavingsTransactionsQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive);
public record SavingsTransactionsPagedResponse(List<SavingsTransactionSummaryDto> Items, int TotalCount, int Page, int PageSize);
