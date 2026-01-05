using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.GroupMemberships.GetGroupMemberships;
using FSH.Module.Microfinance.Contracts.v1.GroupMemberships;

namespace FSH.Module.Microfinance.Features.v1.GroupMemberships.GetGroupMemberships;

public class GetGroupMembershipsHandler(MicrofinanceDbContext context) : IQueryHandler<GetGroupMembershipsQuery, GroupMembershipsPagedResponse>
{
    public async ValueTask<GroupMembershipsPagedResponse> Handle(GetGroupMembershipsQuery query, CancellationToken ct)
    {
        var queryable = context.GroupMemberships.AsQueryable();
        
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
            .Select(x => new GroupMembershipSummaryDto(x.Id, x.Name, x.IsActive))
            .ToListAsync(ct);
        
        return new GroupMembershipsPagedResponse(items, totalCount, query.Page, query.PageSize);
    }
}
