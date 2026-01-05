using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.CreditMemos.GetListCreditMemo;

public sealed record GetCreditMemosQuery(int Page = 1, int PageSize = 10, string? SearchTerm = null, bool? IsActive = null) : IQuery<CreditMemosPagedResponse>;

public sealed record CreditMemosPagedResponse(List<CreditMemoSummaryDto> Items, int TotalCount, int Page, int PageSize);