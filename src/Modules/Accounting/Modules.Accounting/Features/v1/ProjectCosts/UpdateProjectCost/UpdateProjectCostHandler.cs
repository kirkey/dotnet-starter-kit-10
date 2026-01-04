using FSH.Framework.Core.Exceptions;
using FSH.Modules.Accounting.Data;
using Mediator;

namespace FSH.Modules.Accounting.Features.v1.ProjectCosts.UpdateProjectCost;

public record UpdateProjectCostCommand(Guid Id, string Name, string? Description) : ICommand<Guid>;

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
