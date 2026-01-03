using FSH.Framework.Core.Exceptions;
using FSH.Modules.Microfinance.Contracts.v1.ApprovalWorkflows;
using FSH.Modules.Microfinance.Data;

namespace FSH.Modules.Microfinance.Features.v1.ApprovalWorkflows.GetApprovalWorkflow;

public record GetApprovalWorkflowQuery(Guid Id) : IQuery<ApprovalWorkflowDto>;

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
