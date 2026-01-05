using FSH.Framework.Core.Context;
using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;
using FSH.Module.Microfinance.Domain;

using FSH.Module.Microfinance.Contracts.v1.ApprovalWorkflows.CreateApprovalWorkflow;

namespace FSH.Module.Microfinance.Features.v1.ApprovalWorkflows.CreateApprovalWorkflow;

public class CreateApprovalWorkflowHandler(ICurrentUser currentUser,
    MicrofinanceDbContext context) : ICommandHandler<CreateApprovalWorkflowCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateApprovalWorkflowCommand command, CancellationToken ct)
    {
        var entity = ApprovalWorkflow.Create(
            command.Name,
            currentUser.GetTenant() ?? "root",
            currentUser.GetUserId(),
            currentUser.Name ?? "System");
        
        context.ApprovalWorkflows.Add(entity);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
