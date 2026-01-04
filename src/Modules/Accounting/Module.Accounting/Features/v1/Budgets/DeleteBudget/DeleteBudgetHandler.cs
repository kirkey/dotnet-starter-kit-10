using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Data;
using Mediator;

namespace FSH.Module.Accounting.Features.v1.Budgets.DeleteBudget;

public record DeleteBudgetCommand(Guid Id) : ICommand;

public class DeleteBudgetHandler(AccountingDbContext context) : ICommandHandler<DeleteBudgetCommand>
{
    public async ValueTask<Unit> Handle(DeleteBudgetCommand command, CancellationToken ct)
    {
        var entity = await context.Budgets.FindAsync(command.Id, ct)
            ?? throw new NotFoundException("Budget not found");
        
        context.Budgets.Remove(entity);
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
