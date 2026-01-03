using FSH.Framework.Core.Exceptions;
using FSH.Modules.Microfinance.Data;

namespace FSH.Modules.Microfinance.Features.v1.ApprovalWorkflows.UpdateApprovalWorkflow;

public record UpdateApprovalWorkflowCommand(Guid Id, string Name) : ICommand<Guid>;

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
