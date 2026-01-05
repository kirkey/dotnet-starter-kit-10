using FSH.Module.Microfinance.Contracts.v1.MemberGroups;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.MemberGroups.GetMemberGroups;

namespace FSH.Module.Microfinance.Features.v1.MemberGroups.GetMemberGroups;

public class GetMemberGroupsHandler(MicrofinanceDbContext context) : IQueryHandler<GetMemberGroupsQuery, MemberGroupsPagedResponse>
{
    public async ValueTask<MemberGroupsPagedResponse> Handle(GetMemberGroupsQuery query, CancellationToken ct)
    {
        var queryable = context.MemberGroups.AsQueryable();
        
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
            .Select(x => new MemberGroupSummaryDto(x.Id, x.Name, x.IsActive))
            .ToListAsync(ct);
        
        return new MemberGroupsPagedResponse(items, totalCount, query.Page, query.PageSize);
    }
}
