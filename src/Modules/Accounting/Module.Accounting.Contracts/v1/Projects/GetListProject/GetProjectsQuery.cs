using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.Projects.GetListProject;

public record GetProjectsQuery(int Page = 1, int PageSize = 10, string? SearchTerm = null, bool? IsActive = null) : IQuery<ProjectsPagedResponse>;

public record ProjectsPagedResponse(List<ProjectSummaryDto> Items, int TotalCount, int Page, int PageSize);