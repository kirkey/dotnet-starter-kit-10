using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.RetainedEarnings.GetListRetainedEarnings;

public sealed record GetRetainedEarningsQuery(int Page = 1, int PageSize = 10, string? SearchTerm = null, bool? IsActive = null) : IQuery<RetainedEarningsPagedResponse>;

public sealed record RetainedEarningsPagedResponse(List<RetainedEarningsSummaryDto> Items, int TotalCount, int Page, int PageSize);