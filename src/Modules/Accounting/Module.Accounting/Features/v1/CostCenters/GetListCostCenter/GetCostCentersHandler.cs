using FSH.Module.Accounting.Contracts.v1.CostCenters;
using FSH.Module.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Module.Accounting.Features.v1.CostCenters.GetCostCenters;

public record GetCostCentersQuery(
    int Page = 1,
    int PageSize = 10,
    string? SearchTerm = null,
    bool? IsActive = null) : IQuery<CostCentersPagedResponse>;

public record CostCentersPagedResponse(
    List<CostCenterSummaryDto> Items,
    int TotalCount,
    int Page,
    int PageSize);

public class GetCostCentersHandler(AccountingDbContext context) 
    : IQueryHandler<GetCostCentersQuery, CostCentersPagedResponse>
{
    public async ValueTask<CostCentersPagedResponse> Handle(GetCostCentersQuery query, CancellationToken ct)
    {
        var queryable = context.CostCenters.AsQueryable();
        
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
            .Select(x => new CostCenterSummaryDto(x.Id, x.Name, x.IsActive))
            .ToListAsync(ct);
        
        return new CostCentersPagedResponse(items, totalCount, query.Page, query.PageSize);
    }
}
