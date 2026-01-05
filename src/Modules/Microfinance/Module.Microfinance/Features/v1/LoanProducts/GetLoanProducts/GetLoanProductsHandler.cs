using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.LoanProducts.GetLoanProducts;
using FSH.Module.Microfinance.Contracts.v1.LoanProducts;

namespace FSH.Module.Microfinance.Features.v1.LoanProducts.GetLoanProducts;

public class GetLoanProductsHandler(MicrofinanceDbContext context) : IQueryHandler<GetLoanProductsQuery, LoanProductsPagedResponse>
{
    public async ValueTask<LoanProductsPagedResponse> Handle(GetLoanProductsQuery query, CancellationToken ct)
    {
        var queryable = context.LoanProducts.AsQueryable();
        
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
            .Select(x => new LoanProductSummaryDto(x.Id, x.Name, x.IsActive))
            .ToListAsync(ct);
        
        return new LoanProductsPagedResponse(items, totalCount, query.Page, query.PageSize);
    }
}
