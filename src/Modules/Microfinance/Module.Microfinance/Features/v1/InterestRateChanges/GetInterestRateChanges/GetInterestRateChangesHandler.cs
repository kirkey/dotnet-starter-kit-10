using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.InterestRateChanges.GetInterestRateChanges;
using FSH.Module.Microfinance.Contracts.v1.InterestRateChanges;

namespace FSH.Module.Microfinance.Features.v1.InterestRateChanges.GetInterestRateChanges;

public class GetInterestRateChangesHandler(MicrofinanceDbContext context) : IQueryHandler<GetInterestRateChangesQuery, InterestRateChangesPagedResponse>
{
    public async ValueTask<InterestRateChangesPagedResponse> Handle(GetInterestRateChangesQuery query, CancellationToken ct)
    {
        var queryable = context.InterestRateChanges.AsQueryable();
        
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
            .Select(x => new InterestRateChangeSummaryDto(x.Id, x.Name, x.IsActive))
            .ToListAsync(ct);
        
        return new InterestRateChangesPagedResponse(items, totalCount, query.Page, query.PageSize);
    }
}
