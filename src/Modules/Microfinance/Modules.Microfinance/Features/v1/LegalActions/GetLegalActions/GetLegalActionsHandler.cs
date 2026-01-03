using FSH.Modules.Microfinance.Contracts.v1.LegalActions;
using FSH.Modules.Microfinance.Data;

namespace FSH.Modules.Microfinance.Features.v1.LegalActions.GetLegalActions;

public record GetLegalActionsQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive) : IQuery<LegalActionsPagedResponse>;

public class GetLegalActionsHandler(MicrofinanceDbContext context) : IQueryHandler<GetLegalActionsQuery, LegalActionsPagedResponse>
{
    public async ValueTask<LegalActionsPagedResponse> Handle(GetLegalActionsQuery query, CancellationToken ct)
    {
        var queryable = context.LegalActions.AsQueryable();
        
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
            .Select(x => new LegalActionSummaryDto(x.Id, x.Name, x.IsActive))
            .ToListAsync(ct);
        
        return new LegalActionsPagedResponse(items, totalCount, query.Page, query.PageSize);
    }
}
