using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Data;
using Mediator;

namespace FSH.Module.Accounting.Features.v1.Budgets.UpdateBudget;

/// <summary>
/// Command to update budget metadata (Name, Description).
/// </summary>
/// <param name="Id">Budget ID to update</param>
/// <param name="Name">Updated budget name</param>
/// <param name="Description">Updated description or null</param>
public record UpdateBudgetCommand(Guid Id, string Name, string? Description) : ICommand<Guid>;

/// <summary>
/// Handler for updating budget header fields.
/// </summary>
/// <remarks>
/// Responsibility: Find budget by Id, apply updates via entity.Update(...), and persist changes.
/// 
/// Updateable Fields: Name, Description
/// Immutable Fields: Id, CreatedOnUtc, CreatedBy
/// 
/// Permissions: Requires Budget.Update
/// 
/// Exceptions:
/// - NotFoundException: Thrown if budget not found
/// </remarks>
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
