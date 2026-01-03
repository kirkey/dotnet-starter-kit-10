using FSH.Modules.Microfinance.Contracts.v1.Branches;
using FSH.Modules.Microfinance.Data;

namespace FSH.Modules.Microfinance.Features.v1.Branches.GetBranchs;

public record GetBranchsQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive) : IQuery<BranchsPagedResponse>;

public class GetBranchsHandler(MicrofinanceDbContext context) : IQueryHandler<GetBranchsQuery, BranchsPagedResponse>
{
    public async ValueTask<BranchsPagedResponse> Handle(GetBranchsQuery query, CancellationToken ct)
    {
        var queryable = context.Branches.AsQueryable();
        
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
            .Select(x => new BranchSummaryDto(x.Id, x.Name, x.IsActive))
            .ToListAsync(ct);
        
        return new BranchsPagedResponse(items, totalCount, query.Page, query.PageSize);
    }
}
