using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.ProjectCosts.GetListProjectCost;

public record GetProjectCostsQuery(int Page = 1, int PageSize = 10, string? SearchTerm = null, bool? IsActive = null) : IQuery<ProjectCostsPagedResponse>;

public record ProjectCostsPagedResponse(List<ProjectCostSummaryDto> Items, int TotalCount, int Page, int PageSize);