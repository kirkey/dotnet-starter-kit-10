using FSH.Modules.Microfinance.Contracts.v1.CollateralReleases;
using FSH.Modules.Microfinance.Data;

namespace FSH.Modules.Microfinance.Features.v1.CollateralReleases.GetCollateralReleases;

public record GetCollateralReleasesQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive) : IQuery<CollateralReleasesPagedResponse>;

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
