using FSH.Modules.Microfinance.Contracts.v1.FeeCharges;
using FSH.Modules.Microfinance.Data;

namespace FSH.Modules.Microfinance.Features.v1.FeeCharges.GetFeeCharges;

public record GetFeeChargesQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive) : IQuery<FeeChargesPagedResponse>;

public class GetFeeChargesHandler(MicrofinanceDbContext context) : IQueryHandler<GetFeeChargesQuery, FeeChargesPagedResponse>
{
    public async ValueTask<FeeChargesPagedResponse> Handle(GetFeeChargesQuery query, CancellationToken ct)
    {
        var queryable = context.FeeCharges.AsQueryable();
        
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
            .Select(x => new FeeChargeSummaryDto(x.Id, x.Name, x.IsActive))
            .ToListAsync(ct);
        
        return new FeeChargesPagedResponse(items, totalCount, query.Page, query.PageSize);
    }
}
