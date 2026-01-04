using FSH.Module.Microfinance.Contracts.v1.SavingsProducts;
using FSH.Module.Microfinance.Data;

namespace FSH.Module.Microfinance.Features.v1.SavingsProducts.GetSavingsProducts;

public record GetSavingsProductsQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive) : IQuery<SavingsProductsPagedResponse>;

public class GetSavingsProductsHandler(MicrofinanceDbContext context) : IQueryHandler<GetSavingsProductsQuery, SavingsProductsPagedResponse>
{
    public async ValueTask<SavingsProductsPagedResponse> Handle(GetSavingsProductsQuery query, CancellationToken ct)
    {
        var queryable = context.SavingsProducts.AsQueryable();
        
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
            .Select(x => new SavingsProductSummaryDto(x.Id, x.Name, x.IsActive))
            .ToListAsync(ct);
        
        return new SavingsProductsPagedResponse(items, totalCount, query.Page, query.PageSize);
    }
}
