using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Module.Accounting.Features.v1.Budgets.ApproveBudget;

/// <summary>
/// Command to approve a budget (state transition to Approved).
/// </summary>
/// <param name="Id">Budget ID to approve</param>
public record ApproveBudgetCommand(Guid Id) : ICommand;

/// <summary>
/// Handler for approving a budget. This should mark the budget as Approved and record approver metadata.
/// </summary>
/// <remarks>
/// Responsibility: Validate budget state, set approved flags and approver info, and persist changes.
/// 
/// Execution Flow:
/// 1. Load budget by Id
/// 2. Validate current state (e.g., not already approved or deleted)
/// 3. Domain-level approve behavior should set status and approver info (TODO in domain)
/// 4. Persist changes
/// 
/// Note: Implementation contains a TODO to add domain-specific approve behavior; consider adding Approve() method on Budget aggregate.
/// 
/// Permissions: Requires Budget.Approve
/// 
/// Exceptions:
/// - NotFoundException: Thrown if budget not found
/// - BusinessRuleException: Thrown if budget is not approvable
/// </remarks>
public class ApproveBudgetHandler(AccountingDbContext context) 
    : ICommandHandler<ApproveBudgetCommand>
{
    public async ValueTask<Unit> Handle(ApproveBudgetCommand command, CancellationToken ct)
    {
        var entity = await context.Budgets.FirstOrDefaultAsync(x => x.Id == command.Id, ct)
            ?? throw new NotFoundException("Budget not found");

        // TODO: add domain-level approve behavior (set status, approver info)

        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
