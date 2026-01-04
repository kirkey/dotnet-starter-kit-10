namespace FSH.Module.Microfinance.Contracts.v1.InvestmentTransactions;

public record GetInvestmentTransactionQuery(Guid Id);
public record GetInvestmentTransactionsQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive);
public record InvestmentTransactionsPagedResponse(List<InvestmentTransactionSummaryDto> Items, int TotalCount, int Page, int PageSize);
