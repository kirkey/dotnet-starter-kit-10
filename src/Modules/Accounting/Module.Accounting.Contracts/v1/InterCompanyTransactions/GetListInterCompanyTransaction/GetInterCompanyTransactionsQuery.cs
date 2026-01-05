using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.InterCompanyTransactions.GetListInterCompanyTransaction;

public sealed record GetInterCompanyTransactionsQuery(int Page = 1, int PageSize = 10, string? SearchTerm = null, bool? IsActive = null) : IQuery<InterCompanyTransactionsPagedResponse>;

public sealed record InterCompanyTransactionsPagedResponse(List<InterCompanyTransactionSummaryDto> Items, int TotalCount, int Page, int PageSize);