using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Data;
using Mediator;

namespace FSH.Module.Accounting.Features.v1.Budgets.DeleteBudget;

/// <summary>
/// Command to delete a budget header.
/// </summary>
/// <param name="Id">Budget ID to delete</param>
public record DeleteBudgetCommand(Guid Id) : ICommand;

/// <summary>
/// Handler for deleting a budget.
/// </summary>
/// <remarks>
/// Responsibility: Remove budget entity from DbSet and persist. Consider validation to prevent deleting budgets with details or posted activity.
/// 
/// Permissions: Requires Budget.Delete
/// 
/// Exceptions:
/// - NotFoundException: Thrown if budget not found
/// </remarks>
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
