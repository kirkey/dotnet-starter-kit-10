using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.ShareAccounts.GetShareAccounts;
using FSH.Module.Microfinance.Contracts.v1.ShareAccounts;

namespace FSH.Module.Microfinance.Features.v1.ShareAccounts.GetShareAccounts;

public class GetShareAccountsHandler(MicrofinanceDbContext context) : IQueryHandler<GetShareAccountsQuery, ShareAccountsPagedResponse>
{
    public async ValueTask<ShareAccountsPagedResponse> Handle(GetShareAccountsQuery query, CancellationToken ct)
    {
        var queryable = context.ShareAccounts.AsQueryable();
        
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
            .Select(x => new ShareAccountSummaryDto(x.Id, x.Name, x.IsActive))
            .ToListAsync(ct);
        
        return new ShareAccountsPagedResponse(items, totalCount, query.Page, query.PageSize);
    }
}
