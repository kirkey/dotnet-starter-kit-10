using FSH.Framework.Shared.Identity;
using FSH.Module.Accounting.Data;
using FSH.Module.Accounting.Domain;
using Mediator;

namespace FSH.Module.Accounting.Features.v1.ProjectCosts.CreateProjectCost;

public record CreateProjectCostCommand(string Name, string? Description) : ICommand<Guid>;

public class CreateProjectCostHandler(AccountingDbContext context, ICurrentUser currentUser) 
    : ICommandHandler<CreateProjectCostCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateProjectCostCommand command, CancellationToken ct)
    {
        var entity = ProjectCost.Create(
            command.Name,
            currentUser.GetTenant() ?? "root",
            currentUser.GetUserId(),
            currentUser.Name ?? "System",
            command.Description);
        
        context.ProjectCosts.Add(entity);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
