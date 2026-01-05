using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.ApprovalWorkflows.GetApprovalWorkflows;
using FSH.Module.Microfinance.Contracts.v1.ApprovalWorkflows;

namespace FSH.Module.Microfinance.Features.v1.ApprovalWorkflows.GetApprovalWorkflows;

public class GetApprovalWorkflowsHandler(MicrofinanceDbContext context) : IQueryHandler<GetApprovalWorkflowsQuery, ApprovalWorkflowsPagedResponse>
{
    public async ValueTask<ApprovalWorkflowsPagedResponse> Handle(GetApprovalWorkflowsQuery query, CancellationToken ct)
    {
        var queryable = context.ApprovalWorkflows.AsQueryable();
        
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
            .Select(x => new ApprovalWorkflowSummaryDto(x.Id, x.Name, x.IsActive))
            .ToListAsync(ct);
        
        return new ApprovalWorkflowsPagedResponse(items, totalCount, query.Page, query.PageSize);
    }
}
