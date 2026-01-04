using FSH.Module.Microfinance.Contracts.v1.SavingsAccounts;
using FSH.Module.Microfinance.Data;

namespace FSH.Module.Microfinance.Features.v1.SavingsAccounts.GetSavingsAccounts;

public record GetSavingsAccountsQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive) : IQuery<SavingsAccountsPagedResponse>;

public class GetSavingsAccountsHandler(MicrofinanceDbContext context) : IQueryHandler<GetSavingsAccountsQuery, SavingsAccountsPagedResponse>
{
    public async ValueTask<SavingsAccountsPagedResponse> Handle(GetSavingsAccountsQuery query, CancellationToken ct)
    {
        var queryable = context.SavingsAccounts.AsQueryable();
        
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
            .Select(x => new SavingsAccountSummaryDto(x.Id, x.Name, x.IsActive))
            .ToListAsync(ct);
        
        return new SavingsAccountsPagedResponse(items, totalCount, query.Page, query.PageSize);
    }
}
