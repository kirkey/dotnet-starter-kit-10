using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Data;
using Mediator;
using FSH.Module.Accounting.Contracts.v1.ProjectCosts.UpdateProjectCost;

namespace FSH.Module.Accounting.Features.v1.ProjectCosts.UpdateProjectCost;

public class UpdateProjectCostHandler(AccountingDbContext context) : ICommandHandler<UpdateProjectCostCommand, Guid>
{
    public async ValueTask<Guid> Handle(UpdateProjectCostCommand command, CancellationToken ct)
    {
        var entity = await context.ProjectCosts.FindAsync(command.Id, ct)
            ?? throw new NotFoundException("ProjectCost not found");
        
        entity.Update(command.Name, command.Description);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
