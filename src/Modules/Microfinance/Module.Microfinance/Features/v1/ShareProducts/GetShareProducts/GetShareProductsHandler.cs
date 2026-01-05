using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.ShareProducts.GetShareProducts;
using FSH.Module.Microfinance.Contracts.v1.ShareProducts;

namespace FSH.Module.Microfinance.Features.v1.ShareProducts.GetShareProducts;

public class GetShareProductsHandler(MicrofinanceDbContext context) : IQueryHandler<GetShareProductsQuery, ShareProductsPagedResponse>
{
    public async ValueTask<ShareProductsPagedResponse> Handle(GetShareProductsQuery query, CancellationToken ct)
    {
        var queryable = context.ShareProducts.AsQueryable();
        
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
            .Select(x => new ShareProductSummaryDto(x.Id, x.Name, x.IsActive))
            .ToListAsync(ct);
        
        return new ShareProductsPagedResponse(items, totalCount, query.Page, query.PageSize);
    }
}
