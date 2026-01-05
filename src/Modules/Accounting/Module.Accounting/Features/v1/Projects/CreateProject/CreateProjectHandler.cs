using FSH.Framework.Shared.Identity;
using FSH.Module.Accounting.Data;
using FSH.Module.Accounting.Domain;
using Mediator;
using FSH.Module.Accounting.Contracts.v1.Projects.CreateProject;

namespace FSH.Module.Accounting.Features.v1.Projects.CreateProject;

public class CreateProjectHandler(AccountingDbContext context, ICurrentUser currentUser) 
    : ICommandHandler<CreateProjectCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateProjectCommand command, CancellationToken ct)
    {
        var entity = Project.Create(
            command.Name,
            currentUser.GetTenant() ?? "root",
            currentUser.GetUserId(),
            currentUser.Name ?? "System",
            command.Description);
        
        context.Projects.Add(entity);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
