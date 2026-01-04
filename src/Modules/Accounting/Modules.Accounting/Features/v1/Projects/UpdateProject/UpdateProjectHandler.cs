using FSH.Framework.Core.Exceptions;
using FSH.Modules.Accounting.Data;
using Mediator;

namespace FSH.Modules.Accounting.Features.v1.Projects.UpdateProject;

public record UpdateProjectCommand(Guid Id, string Name, string? Description) : ICommand<Guid>;

public class UpdateProjectHandler(AccountingDbContext context) : ICommandHandler<UpdateProjectCommand, Guid>
{
    public async ValueTask<Guid> Handle(UpdateProjectCommand command, CancellationToken ct)
    {
        var entity = await context.Projects.FindAsync(command.Id, ct)
            ?? throw new NotFoundException("Project not found");
        
        entity.Update(command.Name, command.Description);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
