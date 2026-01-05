using FSH.Module.Microfinance.Contracts.v1.CollateralInsurances;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.CollateralInsurances.GetCollateralInsurances;

namespace FSH.Module.Microfinance.Features.v1.CollateralInsurances.GetCollateralInsurances;

public class GetCollateralInsurancesHandler(MicrofinanceDbContext context) : IQueryHandler<GetCollateralInsurancesQuery, CollateralInsurancesPagedResponse>
{
    public async ValueTask<CollateralInsurancesPagedResponse> Handle(GetCollateralInsurancesQuery query, CancellationToken ct)
    {
        var queryable = context.CollateralInsurances.AsQueryable();
        
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
            .Select(x => new CollateralInsuranceSummaryDto(x.Id, x.Name, x.IsActive))
            .ToListAsync(ct);
        
        return new CollateralInsurancesPagedResponse(items, totalCount, query.Page, query.PageSize);
    }
}
