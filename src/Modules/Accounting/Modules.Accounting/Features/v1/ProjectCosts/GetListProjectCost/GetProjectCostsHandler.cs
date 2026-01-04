using FSH.Modules.Accounting.Contracts.v1.ProjectCosts;
using FSH.Modules.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Modules.Accounting.Features.v1.ProjectCosts.GetProjectCosts;

public record GetProjectCostsQuery(
    int Page = 1,
    int PageSize = 10,
    string? SearchTerm = null,
    bool? IsActive = null) : IQuery<ProjectCostsPagedResponse>;

public record ProjectCostsPagedResponse(
    List<ProjectCostSummaryDto> Items,
    int TotalCount,
    int Page,
    int PageSize);

public class GetProjectCostsHandler(AccountingDbContext context) 
    : IQueryHandler<GetProjectCostsQuery, ProjectCostsPagedResponse>
{
    public async ValueTask<ProjectCostsPagedResponse> Handle(GetProjectCostsQuery query, CancellationToken ct)
    {
        var queryable = context.ProjectCosts.AsQueryable();
        
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
            .Select(x => new ProjectCostSummaryDto(x.Id, x.Name, x.IsActive))
            .ToListAsync(ct);
        
        return new ProjectCostsPagedResponse(items, totalCount, query.Page, query.PageSize);
    }
}
