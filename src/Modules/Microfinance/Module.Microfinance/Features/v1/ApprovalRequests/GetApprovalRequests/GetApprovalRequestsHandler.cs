using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.ApprovalRequests.GetApprovalRequests;
using FSH.Module.Microfinance.Contracts.v1.ApprovalRequests;

namespace FSH.Module.Microfinance.Features.v1.ApprovalRequests.GetApprovalRequests;

public class GetApprovalRequestsHandler(MicrofinanceDbContext context) : IQueryHandler<GetApprovalRequestsQuery, ApprovalRequestsPagedResponse>
{
    public async ValueTask<ApprovalRequestsPagedResponse> Handle(GetApprovalRequestsQuery query, CancellationToken ct)
    {
        var queryable = context.ApprovalRequests.AsQueryable();
        
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
            .Select(x => new ApprovalRequestSummaryDto(x.Id, x.Name, x.IsActive))
            .ToListAsync(ct);
        
        return new ApprovalRequestsPagedResponse(items, totalCount, query.Page, query.PageSize);
    }
}
