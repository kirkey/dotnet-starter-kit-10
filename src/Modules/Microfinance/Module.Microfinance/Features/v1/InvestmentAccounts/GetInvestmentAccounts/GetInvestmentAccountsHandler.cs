using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.InvestmentAccounts.GetInvestmentAccounts;
using FSH.Module.Microfinance.Contracts.v1.InvestmentAccounts;

namespace FSH.Module.Microfinance.Features.v1.InvestmentAccounts.GetInvestmentAccounts;

public class GetInvestmentAccountsHandler(MicrofinanceDbContext context) : IQueryHandler<GetInvestmentAccountsQuery, InvestmentAccountsPagedResponse>
{
    public async ValueTask<InvestmentAccountsPagedResponse> Handle(GetInvestmentAccountsQuery query, CancellationToken ct)
    {
        var queryable = context.InvestmentAccounts.AsQueryable();
        
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
            .Select(x => new InvestmentAccountSummaryDto(x.Id, x.Name, x.IsActive))
            .ToListAsync(ct);
        
        return new InvestmentAccountsPagedResponse(items, totalCount, query.Page, query.PageSize);
    }
}
