using FSH.Module.Microfinance.Contracts.v1.CashVaults;
using FSH.Module.Microfinance.Data;

namespace FSH.Module.Microfinance.Features.v1.CashVaults.GetCashVaults;

public record GetCashVaultsQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive) : IQuery<CashVaultsPagedResponse>;

public class GetCashVaultsHandler(MicrofinanceDbContext context) : IQueryHandler<GetCashVaultsQuery, CashVaultsPagedResponse>
{
    public async ValueTask<CashVaultsPagedResponse> Handle(GetCashVaultsQuery query, CancellationToken ct)
    {
        var queryable = context.CashVaults.AsQueryable();
        
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
            .Select(x => new CashVaultSummaryDto(x.Id, x.Name, x.IsActive))
            .ToListAsync(ct);
        
        return new CashVaultsPagedResponse(items, totalCount, query.Page, query.PageSize);
    }
}
