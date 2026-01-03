using FSH.Modules.Microfinance.Contracts.v1.InsuranceProducts;
using FSH.Modules.Microfinance.Data;

namespace FSH.Modules.Microfinance.Features.v1.InsuranceProducts.GetInsuranceProducts;

public record GetInsuranceProductsQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive) : IQuery<InsuranceProductsPagedResponse>;

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
