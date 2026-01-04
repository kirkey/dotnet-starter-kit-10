using FSH.Modules.Accounting.Contracts.v1.FixedAssets;
using FSH.Modules.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Modules.Accounting.Features.v1.FixedAssets.GetFixedAssets;

public record GetFixedAssetsQuery(
    int Page = 1,
    int PageSize = 10,
    string? SearchTerm = null,
    bool? IsActive = null) : IQuery<FixedAssetsPagedResponse>;

public record FixedAssetsPagedResponse(
    List<FixedAssetSummaryDto> Items,
    int TotalCount,
    int Page,
    int PageSize);

public class GetFixedAssetsHandler(AccountingDbContext context) 
    : IQueryHandler<GetFixedAssetsQuery, FixedAssetsPagedResponse>
{
    public async ValueTask<FixedAssetsPagedResponse> Handle(GetFixedAssetsQuery query, CancellationToken ct)
    {
        var queryable = context.FixedAssets.AsQueryable();
        
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
            .Select(x => new FixedAssetSummaryDto(x.Id, x.Name, x.IsActive))
            .ToListAsync(ct);
        
        return new FixedAssetsPagedResponse(items, totalCount, query.Page, query.PageSize);
    }
}
