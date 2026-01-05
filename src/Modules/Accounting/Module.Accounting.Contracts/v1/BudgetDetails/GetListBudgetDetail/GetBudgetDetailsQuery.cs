using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.BudgetDetails.GetListBudgetDetail;

public sealed record GetBudgetDetailsQuery(int Page = 1, int PageSize = 10, string? SearchTerm = null, bool? IsActive = null) : IQuery<BudgetDetailsPagedResponse>;

public sealed record BudgetDetailsPagedResponse(List<BudgetDetailSummaryDto> Items, int TotalCount, int Page, int PageSize);