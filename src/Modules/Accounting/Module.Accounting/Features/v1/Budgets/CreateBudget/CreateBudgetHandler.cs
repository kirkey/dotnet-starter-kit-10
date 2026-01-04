using FSH.Framework.Shared.Identity;
using FSH.Module.Accounting.Data;
using FSH.Module.Accounting.Domain;
using Mediator;

namespace FSH.Module.Accounting.Features.v1.Budgets.CreateBudget;

public record CreateBudgetCommand(string Name, string? Description) : ICommand<Guid>;

public class CreateBudgetHandler(AccountingDbContext context, ICurrentUser currentUser) 
    : ICommandHandler<CreateBudgetCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateBudgetCommand command, CancellationToken ct)
    {
        var entity = Budget.Create(
            command.Name,
            currentUser.GetTenant() ?? "root",
            currentUser.GetUserId(),
            currentUser.Name ?? "System",
            command.Description);
        
        context.Budgets.Add(entity);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
