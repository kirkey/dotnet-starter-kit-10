using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.CostCenters.GetListCostCenter;

public sealed record GetCostCentersQuery(
    int Page = 1,
    int PageSize = 10,
    string? SearchTerm = null,
    bool? IsActive = null) : IQuery<CostCentersPagedResponse>;

public sealed record CostCentersPagedResponse(
    List<CostCenterSummaryDto> Items,
    int TotalCount,
    int Page,
    int PageSize);
