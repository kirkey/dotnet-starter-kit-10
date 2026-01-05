using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.SavingsTransactions.GetSavingsTransactions;

public sealed record GetSavingsTransactionsQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive) : IQuery<SavingsTransactionsPagedResponse>;

public sealed record SavingsTransactionsPagedResponse(List<SavingsTransactionSummaryDto> Items, int TotalCount, int Page, int PageSize);
