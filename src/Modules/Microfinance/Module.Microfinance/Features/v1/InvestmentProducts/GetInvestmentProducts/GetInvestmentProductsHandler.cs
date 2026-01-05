using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.InvestmentProducts.GetInvestmentProducts;
using FSH.Module.Microfinance.Contracts.v1.InvestmentProducts;

namespace FSH.Module.Microfinance.Features.v1.InvestmentProducts.GetInvestmentProducts;

public class GetInvestmentProductsHandler(MicrofinanceDbContext context) : IQueryHandler<GetInvestmentProductsQuery, InvestmentProductsPagedResponse>
{
    public async ValueTask<InvestmentProductsPagedResponse> Handle(GetInvestmentProductsQuery query, CancellationToken ct)
    {
        var queryable = context.InvestmentProducts.AsQueryable();
        
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
            .Select(x => new InvestmentProductSummaryDto(x.Id, x.Name, x.IsActive))
            .ToListAsync(ct);
        
        return new InvestmentProductsPagedResponse(items, totalCount, query.Page, query.PageSize);
    }
}
