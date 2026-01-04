using FSH.Framework.Core.Exceptions;
using FSH.Modules.Accounting.Data;
using Mediator;

namespace FSH.Modules.Accounting.Features.v1.Projects.DeleteProject;

public record DeleteProjectCommand(Guid Id) : ICommand;

public class DeleteProjectHandler(AccountingDbContext context) : ICommandHandler<DeleteProjectCommand>
{
    public async ValueTask<Unit> Handle(DeleteProjectCommand command, CancellationToken ct)
    {
        var entity = await context.Projects.FindAsync(command.Id, ct)
            ?? throw new NotFoundException("Project not found");
        
        context.Projects.Remove(entity);
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
