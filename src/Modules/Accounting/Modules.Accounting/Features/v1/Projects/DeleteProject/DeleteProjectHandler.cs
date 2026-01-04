using FSH.Framework.Core.Exceptions;
using FSH.Modules.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Modules.Accounting.Features.v1.Projects.DeleteProject;

public record DeleteProjectCommand(Guid Id) : ICommand;

public class DeleteProjectHandler(AccountingDbContext context) : ICommandHandler<DeleteProjectCommand>
{
    public async ValueTask<Unit> Handle(DeleteProjectCommand command, CancellationToken ct)
    {
        var entity = await context.Projects.FindAsync(command.Id, ct)
            ?? throw new NotFoundException("Project not found");

        // Business rule: Cannot delete projects with cost entries
        var hasCosts = await context.ProjectCostEntries.AnyAsync(x => x.ProjectId == command.Id, ct).ConfigureAwait(false);
        if (hasCosts)
            throw new BadRequestException("Cannot delete project with existing cost entries. Please remove all cost entries first.");
        
        context.Projects.Remove(entity);
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
