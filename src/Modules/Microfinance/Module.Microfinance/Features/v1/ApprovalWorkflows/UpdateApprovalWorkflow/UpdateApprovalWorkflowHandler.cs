using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.ApprovalWorkflows.UpdateApprovalWorkflow;

namespace FSH.Module.Microfinance.Features.v1.ApprovalWorkflows.UpdateApprovalWorkflow;

public class UpdateApprovalWorkflowHandler(MicrofinanceDbContext context) : ICommandHandler<UpdateApprovalWorkflowCommand, Guid>
{
    public async ValueTask<Guid> Handle(UpdateApprovalWorkflowCommand command, CancellationToken ct)
    {
        var entity = await context.ApprovalWorkflows.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("ApprovalWorkflow not found");
        
        entity.Update(command.Name);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
