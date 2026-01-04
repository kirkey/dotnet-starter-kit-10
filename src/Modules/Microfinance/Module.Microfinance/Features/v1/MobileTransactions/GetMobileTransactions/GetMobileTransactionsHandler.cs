using FSH.Module.Microfinance.Contracts.v1.MobileTransactions;
using FSH.Module.Microfinance.Data;

namespace FSH.Module.Microfinance.Features.v1.MobileTransactions.GetMobileTransactions;

public record GetMobileTransactionsQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive) : IQuery<MobileTransactionsPagedResponse>;

public class GetMobileTransactionsHandler(MicrofinanceDbContext context) : IQueryHandler<GetMobileTransactionsQuery, MobileTransactionsPagedResponse>
{
    public async ValueTask<MobileTransactionsPagedResponse> Handle(GetMobileTransactionsQuery query, CancellationToken ct)
    {
        var queryable = context.MobileTransactions.AsQueryable();
        
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
            .Select(x => new MobileTransactionSummaryDto(x.Id, x.Name, x.IsActive))
            .ToListAsync(ct);
        
        return new MobileTransactionsPagedResponse(items, totalCount, query.Page, query.PageSize);
    }
}
