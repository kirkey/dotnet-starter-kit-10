using FSH.Modules.Microfinance.Contracts.v1.DebtSettlements;
using FSH.Modules.Microfinance.Data;

namespace FSH.Modules.Microfinance.Features.v1.DebtSettlements.GetDebtSettlements;

public record GetDebtSettlementsQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive) : IQuery<DebtSettlementsPagedResponse>;

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
