using FSH.Module.Microfinance.Contracts.v1.DebtSettlements;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.DebtSettlements.GetDebtSettlements;

namespace FSH.Module.Microfinance.Features.v1.DebtSettlements.GetDebtSettlements;

public class GetDebtSettlementsHandler(MicrofinanceDbContext context) : IQueryHandler<GetDebtSettlementsQuery, DebtSettlementsPagedResponse>
{
    public async ValueTask<DebtSettlementsPagedResponse> Handle(GetDebtSettlementsQuery query, CancellationToken ct)
    {
        var queryable = context.DebtSettlements.AsQueryable();
        
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
            .Select(x => new DebtSettlementSummaryDto(x.Id, x.Name, x.IsActive))
            .ToListAsync(ct);
        
        return new DebtSettlementsPagedResponse(items, totalCount, query.Page, query.PageSize);
    }
}
