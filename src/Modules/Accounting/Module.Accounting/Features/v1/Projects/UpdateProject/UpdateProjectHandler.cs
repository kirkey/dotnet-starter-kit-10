using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Data;
using Mediator;
using FSH.Module.Accounting.Contracts.v1.Projects.UpdateProject;

namespace FSH.Module.Accounting.Features.v1.Projects.UpdateProject;

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
