using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Contracts.v1.ApprovalWorkflows;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.ApprovalWorkflows.GetApprovalWorkflow;

namespace FSH.Module.Microfinance.Features.v1.ApprovalWorkflows.GetApprovalWorkflow;

public class GetApprovalWorkflowHandler(MicrofinanceDbContext context) : IQueryHandler<GetApprovalWorkflowQuery, ApprovalWorkflowDto>
{
    public async ValueTask<ApprovalWorkflowDto> Handle(GetApprovalWorkflowQuery query, CancellationToken ct)
    {
        var entity = await context.ApprovalWorkflows
            .Where(x => x.Id == query.Id)
            .Select(x => new ApprovalWorkflowDto(x.Id, x.Name, x.IsActive, x.CreatedOnUtc))
            .FirstOrDefaultAsync(ct);
        
        return entity ?? throw new NotFoundException("ApprovalWorkflow not found");
    }
}
