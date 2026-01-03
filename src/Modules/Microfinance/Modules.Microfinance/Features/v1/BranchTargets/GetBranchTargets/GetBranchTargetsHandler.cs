using FSH.Modules.Microfinance.Contracts.v1.BranchTargets;
using FSH.Modules.Microfinance.Data;

namespace FSH.Modules.Microfinance.Features.v1.BranchTargets.GetBranchTargets;

public record GetBranchTargetsQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive) : IQuery<BranchTargetsPagedResponse>;

public class GetBranchTargetsHandler(MicrofinanceDbContext context) : IQueryHandler<GetBranchTargetsQuery, BranchTargetsPagedResponse>
{
    public async ValueTask<BranchTargetsPagedResponse> Handle(GetBranchTargetsQuery query, CancellationToken ct)
    {
        var queryable = context.BranchTargets.AsQueryable();
        
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
            .Select(x => new BranchTargetSummaryDto(x.Id, x.Name, x.IsActive))
            .ToListAsync(ct);
        
        return new BranchTargetsPagedResponse(items, totalCount, query.Page, query.PageSize);
    }
}
