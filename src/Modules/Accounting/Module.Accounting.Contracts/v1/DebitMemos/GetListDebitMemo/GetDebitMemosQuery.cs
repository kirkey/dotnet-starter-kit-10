using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.DebitMemos.GetListDebitMemo;

public sealed record GetDebitMemosQuery(int Page = 1, int PageSize = 10, string? SearchTerm = null, bool? IsActive = null) : IQuery<DebitMemosPagedResponse>;

public sealed record DebitMemosPagedResponse(List<FSH.Module.Accounting.Contracts.v1.DebitMemos.DebitMemoSummaryDto> Items, int TotalCount, int Page, int PageSize);