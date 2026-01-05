using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.ShareTransactions.GetShareTransactions;

public sealed record GetShareTransactionsQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive) : IQuery<ShareTransactionsPagedResponse>;

public sealed record ShareTransactionsPagedResponse(List<ShareTransactionSummaryDto> Items, int TotalCount, int Page, int PageSize);
