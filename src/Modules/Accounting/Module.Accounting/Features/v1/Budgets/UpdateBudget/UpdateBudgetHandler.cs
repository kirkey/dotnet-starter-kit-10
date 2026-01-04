using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Data;
using Mediator;

namespace FSH.Module.Accounting.Features.v1.Budgets.UpdateBudget;

public record UpdateBudgetCommand(Guid Id, string Name, string? Description) : ICommand<Guid>;

public class UpdateBudgetHandler(AccountingDbContext context) : ICommandHandler<UpdateBudgetCommand, Guid>
{
    public async ValueTask<Guid> Handle(UpdateBudgetCommand command, CancellationToken ct)
    {
        var entity = await context.Budgets.FindAsync(command.Id, ct)
            ?? throw new NotFoundException("Budget not found");
        
        entity.Update(command.Name, command.Description);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
