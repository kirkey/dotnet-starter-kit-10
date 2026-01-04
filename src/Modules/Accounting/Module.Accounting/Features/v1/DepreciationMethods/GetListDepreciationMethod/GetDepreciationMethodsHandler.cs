using FSH.Module.Accounting.Contracts.v1.DepreciationMethods;
using FSH.Module.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Module.Accounting.Features.v1.DepreciationMethods.GetDepreciationMethods;

public record GetDepreciationMethodsQuery(
    int Page = 1,
    int PageSize = 10,
    string? SearchTerm = null,
    bool? IsActive = null) : IQuery<DepreciationMethodsPagedResponse>;

public record DepreciationMethodsPagedResponse(
    List<DepreciationMethodSummaryDto> Items,
    int TotalCount,
    int Page,
    int PageSize);

public class GetDepreciationMethodsHandler(AccountingDbContext context) 
    : IQueryHandler<GetDepreciationMethodsQuery, DepreciationMethodsPagedResponse>
{
    public async ValueTask<DepreciationMethodsPagedResponse> Handle(GetDepreciationMethodsQuery query, CancellationToken ct)
    {
        var queryable = context.DepreciationMethods.AsQueryable();
        
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
            .Select(x => new DepreciationMethodSummaryDto(x.Id, x.Name, x.IsActive))
            .ToListAsync(ct);
        
        return new DepreciationMethodsPagedResponse(items, totalCount, query.Page, query.PageSize);
    }
}
