using FSH.Modules.Microfinance.Contracts.v1.ShareAccounts;
using FSH.Modules.Microfinance.Data;

namespace FSH.Modules.Microfinance.Features.v1.ShareAccounts.GetShareAccounts;

public record GetShareAccountsQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive) : IQuery<ShareAccountsPagedResponse>;

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
