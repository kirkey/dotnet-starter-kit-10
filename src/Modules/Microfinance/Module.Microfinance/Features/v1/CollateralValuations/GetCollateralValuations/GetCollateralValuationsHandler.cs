using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.CollateralValuations.GetCollateralValuations;
using FSH.Module.Microfinance.Contracts.v1.CollateralValuations;

namespace FSH.Module.Microfinance.Features.v1.CollateralValuations.GetCollateralValuations;

public class GetCollateralValuationsHandler(MicrofinanceDbContext context) : IQueryHandler<GetCollateralValuationsQuery, CollateralValuationsPagedResponse>
{
    public async ValueTask<CollateralValuationsPagedResponse> Handle(GetCollateralValuationsQuery query, CancellationToken ct)
    {
        var queryable = context.CollateralValuations.AsQueryable();
        
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
            .Select(x => new CollateralValuationSummaryDto(x.Id, x.Name, x.IsActive))
            .ToListAsync(ct);
        
        return new CollateralValuationsPagedResponse(items, totalCount, query.Page, query.PageSize);
    }
}
