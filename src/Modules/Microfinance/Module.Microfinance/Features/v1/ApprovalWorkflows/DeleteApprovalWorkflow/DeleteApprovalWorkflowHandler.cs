using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.ApprovalWorkflows.DeleteApprovalWorkflow;

namespace FSH.Module.Microfinance.Features.v1.ApprovalWorkflows.DeleteApprovalWorkflow;

public class DeleteApprovalWorkflowHandler(MicrofinanceDbContext context) : ICommandHandler<DeleteApprovalWorkflowCommand>
{
    public async ValueTask<Unit> Handle(DeleteApprovalWorkflowCommand command, CancellationToken ct)
    {
        var entity = await context.ApprovalWorkflows.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("ApprovalWorkflow not found");
        
        context.ApprovalWorkflows.Remove(entity);
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
