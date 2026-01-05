using FSH.Module.Microfinance.Contracts.v1.CollateralReleases;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.CollateralReleases.GetCollateralReleases;

namespace FSH.Module.Microfinance.Features.v1.CollateralReleases.GetCollateralReleases;

public class GetCollateralReleasesHandler(MicrofinanceDbContext context) : IQueryHandler<GetCollateralReleasesQuery, CollateralReleasesPagedResponse>
{
    public async ValueTask<CollateralReleasesPagedResponse> Handle(GetCollateralReleasesQuery query, CancellationToken ct)
    {
        var queryable = context.CollateralReleases.AsQueryable();
        
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
            .Select(x => new CollateralReleaseSummaryDto(x.Id, x.Name, x.IsActive))
            .ToListAsync(ct);
        
        return new CollateralReleasesPagedResponse(items, totalCount, query.Page, query.PageSize);
    }
}
