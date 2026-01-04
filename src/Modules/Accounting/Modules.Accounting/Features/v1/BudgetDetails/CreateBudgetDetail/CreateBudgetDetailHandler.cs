using FSH.Framework.Core.Identity;
using FSH.Modules.Accounting.Data;
using FSH.Modules.Accounting.Domain;
using Mediator;

namespace FSH.Modules.Accounting.Features.v1.BudgetDetails.CreateBudgetDetail;

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
