using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.Branches.GetBranchs;
using FSH.Module.Microfinance.Contracts.v1.Branches;

namespace FSH.Module.Microfinance.Features.v1.Branches.GetBranchs;

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
