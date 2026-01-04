using FSH.Modules.Accounting.Contracts.v1.Projects;
using FSH.Modules.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Modules.Accounting.Features.v1.Projects.GetProjects;

public record GetProjectsQuery(
    int Page = 1,
    int PageSize = 10,
    string? SearchTerm = null,
    bool? IsActive = null) : IQuery<ProjectsPagedResponse>;

public record ProjectsPagedResponse(
    List<ProjectSummaryDto> Items,
    int TotalCount,
    int Page,
    int PageSize);

public class GetProjectsHandler(AccountingDbContext context) 
    : IQueryHandler<GetProjectsQuery, ProjectsPagedResponse>
{
    public async ValueTask<ProjectsPagedResponse> Handle(GetProjectsQuery query, CancellationToken ct)
    {
        var queryable = context.Projects.AsQueryable();
        
        if (!string.IsNullOrWhiteSpace(query.SearchTerm))
        {
            queryable = queryable.Where(x => x.Name.Contains(query.SearchTerm));
        }
        
        if (query.IsActive.HasValue)
        {
            queryable = queryable.Where(x => x.IsActive == query.IsActive.Value);
        }
        
        var totalCount = await queryable.CountAsync(ct);
        
        var items = await queryable
            .OrderByDescending(x => x.CreatedOnUtc)
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .Select(x => new ProjectSummaryDto(x.Id, x.Name, x.IsActive))
            .ToListAsync(ct);
        
        return new ProjectsPagedResponse(items, totalCount, query.Page, query.PageSize);
    }
}
