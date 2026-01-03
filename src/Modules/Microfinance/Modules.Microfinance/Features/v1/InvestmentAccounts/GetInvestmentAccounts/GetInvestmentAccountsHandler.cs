using FSH.Modules.Microfinance.Contracts.v1.InvestmentAccounts;
using FSH.Modules.Microfinance.Data;

namespace FSH.Modules.Microfinance.Features.v1.InvestmentAccounts.GetInvestmentAccounts;

public record GetInvestmentAccountsQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive) : IQuery<InvestmentAccountsPagedResponse>;

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
