using FSH.Framework.Core.Identity;
using FSH.Modules.Accounting.Data;
using FSH.Modules.Accounting.Domain;
using Mediator;

namespace FSH.Modules.Accounting.Features.v1.Projects.CreateProject;

public record CreateProjectCommand(string Name, string? Description) : ICommand<Guid>;

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
