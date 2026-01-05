using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.DeferredRevenue.GetDeferredRevenue;

public record GetDeferredRevenueQuery(
    int Page = 1,
    int PageSize = 10,
    string? SearchTerm = null,
    bool? IsActive = null) : IQuery<DeferredRevenuePagedResponse>;

public record DeferredRevenuePagedResponse(
    List<DeferredRevenueSummaryDto> Items,
    int TotalCount,
    int Page,
    int PageSize);