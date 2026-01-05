using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.FixedDeposits.GetFixedDeposits;
using FSH.Module.Microfinance.Contracts.v1.FixedDeposits;

namespace FSH.Module.Microfinance.Features.v1.FixedDeposits.GetFixedDeposits;

public class GetFixedDepositsHandler(MicrofinanceDbContext context) : IQueryHandler<GetFixedDepositsQuery, FixedDepositsPagedResponse>
{
    public async ValueTask<FixedDepositsPagedResponse> Handle(GetFixedDepositsQuery query, CancellationToken ct)
    {
        var queryable = context.FixedDeposits.AsQueryable();
        
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
            .Select(x => new FixedDepositSummaryDto(x.Id, x.Name, x.IsActive))
            .ToListAsync(ct);
        
        return new FixedDepositsPagedResponse(items, totalCount, query.Page, query.PageSize);
    }
}
