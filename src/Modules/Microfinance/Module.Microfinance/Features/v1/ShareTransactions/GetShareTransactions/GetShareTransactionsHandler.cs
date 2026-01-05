using FSH.Module.Microfinance.Contracts.v1.ShareTransactions;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.ShareTransactions.GetShareTransactions;

namespace FSH.Module.Microfinance.Features.v1.ShareTransactions.GetShareTransactions;

public class GetShareTransactionsHandler(MicrofinanceDbContext context) : IQueryHandler<GetShareTransactionsQuery, ShareTransactionsPagedResponse>
{
    public async ValueTask<ShareTransactionsPagedResponse> Handle(GetShareTransactionsQuery query, CancellationToken ct)
    {
        var queryable = context.ShareTransactions.AsQueryable();
        
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
            .Select(x => new ShareTransactionSummaryDto(x.Id, x.Name, x.IsActive))
            .ToListAsync(ct);
        
        return new ShareTransactionsPagedResponse(items, totalCount, query.Page, query.PageSize);
    }
}
