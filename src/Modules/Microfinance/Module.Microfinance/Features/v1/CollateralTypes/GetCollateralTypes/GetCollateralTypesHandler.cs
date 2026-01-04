using FSH.Module.Microfinance.Contracts.v1.CollateralTypes;
using FSH.Module.Microfinance.Data;

namespace FSH.Module.Microfinance.Features.v1.CollateralTypes.GetCollateralTypes;

public record GetCollateralTypesQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive) : IQuery<CollateralTypesPagedResponse>;

public class GetCollateralTypesHandler(MicrofinanceDbContext context) : IQueryHandler<GetCollateralTypesQuery, CollateralTypesPagedResponse>
{
    public async ValueTask<CollateralTypesPagedResponse> Handle(GetCollateralTypesQuery query, CancellationToken ct)
    {
        var queryable = context.CollateralTypes.AsQueryable();
        
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
            .Select(x => new CollateralTypeSummaryDto(x.Id, x.Name, x.IsActive))
            .ToListAsync(ct);
        
        return new CollateralTypesPagedResponse(items, totalCount, query.Page, query.PageSize);
    }
}
