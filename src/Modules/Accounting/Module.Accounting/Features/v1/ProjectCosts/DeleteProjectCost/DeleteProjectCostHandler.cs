using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Module.Accounting.Features.v1.ProjectCosts.DeleteProjectCost;

public record DeleteProjectCostCommand(Guid Id) : ICommand;

public class DeleteProjectCostHandler(AccountingDbContext context) : ICommandHandler<DeleteProjectCostCommand>
{
    public async ValueTask<Unit> Handle(DeleteProjectCostCommand command, CancellationToken ct)
    {
        var entity = await context.ProjectCosts.FindAsync(command.Id, ct)
            ?? throw new NotFoundException("ProjectCost not found");

        // TODO: enforce deletion constraints once ProjectCostEntry is implemented
        // (Currently there is no ProjectCostEntry entity; re-add this check when implemented)
        
        context.ProjectCosts.Remove(entity);
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
} 
