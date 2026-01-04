using FSH.Modules.Accounting.Contracts.v1.DeferredRevenue;
using FSH.Modules.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Modules.Accounting.Features.v1.DeferredRevenue.GetDeferredRevenue;

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

public class GetDeferredRevenueHandler(AccountingDbContext context) 
    : IQueryHandler<GetDeferredRevenueQuery, DeferredRevenuePagedResponse>
{
    public async ValueTask<DeferredRevenuePagedResponse> Handle(GetDeferredRevenueQuery query, CancellationToken ct)
    {
        var queryable = context.DeferredRevenue.AsQueryable();
        
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
            .Select(x => new DeferredRevenueSummaryDto(x.Id, x.Name, x.IsActive))
            .ToListAsync(ct);
        
        return new DeferredRevenuePagedResponse(items, totalCount, query.Page, query.PageSize);
    }
}
