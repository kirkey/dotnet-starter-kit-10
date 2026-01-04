using FSH.Framework.Shared.Identity;
using FSH.Module.Accounting.Data;
using FSH.Module.Accounting.Domain;
using Mediator;

namespace FSH.Module.Accounting.Features.v1.BudgetDetails.CreateBudgetDetail;

public record CreateBudgetDetailCommand(string Name, string? Description) : ICommand<Guid>;

public class CreateBudgetDetailHandler(AccountingDbContext context, ICurrentUser currentUser) 
    : ICommandHandler<CreateBudgetDetailCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateBudgetDetailCommand command, CancellationToken ct)
    {
        var entity = BudgetDetail.Create(
            command.Name,
            currentUser.GetTenant() ?? "root",
            currentUser.GetUserId(),
            currentUser.Name ?? "System",
            command.Description);
        
        context.BudgetDetails.Add(entity);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
