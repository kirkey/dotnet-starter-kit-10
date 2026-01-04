using FSH.Module.Microfinance.Contracts.v1.MobileWallets;
using FSH.Module.Microfinance.Data;

namespace FSH.Module.Microfinance.Features.v1.MobileWallets.GetMobileWallets;

public record GetMobileWalletsQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive) : IQuery<MobileWalletsPagedResponse>;

public class GetMobileWalletsHandler(MicrofinanceDbContext context) : IQueryHandler<GetMobileWalletsQuery, MobileWalletsPagedResponse>
{
    public async ValueTask<MobileWalletsPagedResponse> Handle(GetMobileWalletsQuery query, CancellationToken ct)
    {
        var queryable = context.MobileWallets.AsQueryable();
        
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
            .Select(x => new MobileWalletSummaryDto(x.Id, x.Name, x.IsActive))
            .ToListAsync(ct);
        
        return new MobileWalletsPagedResponse(items, totalCount, query.Page, query.PageSize);
    }
}
