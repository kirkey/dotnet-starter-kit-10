using FSH.Framework.Core.Exceptions;
using FSH.Modules.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Modules.Accounting.Features.v1.ProjectCosts.DeleteProjectCost;

public record DeleteProjectCostCommand(Guid Id) : ICommand;

public class DeleteProjectCostHandler(AccountingDbContext context) : ICommandHandler<DeleteProjectCostCommand>
{
    public async ValueTask<Unit> Handle(DeleteProjectCostCommand command, CancellationToken ct)
    {
        var entity = await context.ProjectCosts.FindAsync(command.Id, ct)
            ?? throw new NotFoundException("ProjectCost not found");

        // Business rule: cannot delete a project cost that has project cost entries
        var hasEntries = await context.ProjectCostEntries.AnyAsync(e => e.ProjectCostId == command.Id, ct).ConfigureAwait(false);
        if (hasEntries)
            throw new BadRequestException("Cannot delete project cost with existing cost entries. Please remove or reassign the entries first.");
        
        context.ProjectCosts.Remove(entity);
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
} 
