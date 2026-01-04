using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Module.Accounting.Features.v1.Projects.DeleteProject;

public record DeleteProjectCommand(Guid Id) : ICommand;

public class DeleteProjectHandler(AccountingDbContext context) : ICommandHandler<DeleteProjectCommand>
{
    public async ValueTask<Unit> Handle(DeleteProjectCommand command, CancellationToken ct)
    {
        var entity = await context.Projects.FindAsync(command.Id, ct)
            ?? throw new NotFoundException("Project not found");

        // Business rule: Cannot delete projects with journal lines or other records referencing the project
        var hasCosts = await context.JournalEntryLines.AnyAsync(x => x.ProjectId == command.Id, ct).ConfigureAwait(false);
        if (hasCosts)
            throw new BadRequestException("Cannot delete project with existing journal lines or related cost entries. Please remove all dependent records first.");
        
        context.Projects.Remove(entity);
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
