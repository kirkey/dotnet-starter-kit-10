using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.InsuranceProducts.GetInsuranceProducts;
using FSH.Module.Microfinance.Contracts.v1.InsuranceProducts;

namespace FSH.Module.Microfinance.Features.v1.InsuranceProducts.GetInsuranceProducts;

public class GetInsuranceProductsHandler(MicrofinanceDbContext context) : IQueryHandler<GetInsuranceProductsQuery, InsuranceProductsPagedResponse>
{
    public async ValueTask<InsuranceProductsPagedResponse> Handle(GetInsuranceProductsQuery query, CancellationToken ct)
    {
        var queryable = context.InsuranceProducts.AsQueryable();
        
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
            .Select(x => new InsuranceProductSummaryDto(x.Id, x.Name, x.IsActive))
            .ToListAsync(ct);
        
        return new InsuranceProductsPagedResponse(items, totalCount, query.Page, query.PageSize);
    }
}
