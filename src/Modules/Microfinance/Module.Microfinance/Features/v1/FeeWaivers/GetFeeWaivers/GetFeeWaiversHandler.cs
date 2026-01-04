using FSH.Module.Microfinance.Contracts.v1.FeeWaivers;
using FSH.Module.Microfinance.Data;

namespace FSH.Module.Microfinance.Features.v1.FeeWaivers.GetFeeWaivers;

public record GetFeeWaiversQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive) : IQuery<FeeWaiversPagedResponse>;

public class GetFeeWaiversHandler(MicrofinanceDbContext context) : IQueryHandler<GetFeeWaiversQuery, FeeWaiversPagedResponse>
{
    public async ValueTask<FeeWaiversPagedResponse> Handle(GetFeeWaiversQuery query, CancellationToken ct)
    {
        var queryable = context.FeeWaivers.AsQueryable();
        
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
            .Select(x => new FeeWaiverSummaryDto(x.Id, x.Name, x.IsActive))
            .ToListAsync(ct);
        
        return new FeeWaiversPagedResponse(items, totalCount, query.Page, query.PageSize);
    }
}
