using FSH.Framework.Core.Exceptions;
using FSH.Framework.Shared.Identity;
using FSH.Module.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Module.Accounting.Features.v1.BankReconciliations.ApproveBankReconciliation;

/// <summary>
/// Command to approve and finalize a bank reconciliation, applying reconciled items.
/// </summary>
/// <param name="Id">BankReconciliation ID to approve</param>
public record ApproveBankReconciliationCommand(Guid Id) : ICommand;

/// <summary>
/// Handler for approving a bank reconciliation. This performs the reconciliation operation and records the approver.
/// </summary>
/// <remarks>
/// Responsibility: Transition the reconciliation to the Reconciled/Approved state, perform any necessary GL adjustments, and persist reconciled lines.
/// 
/// Execution Flow:
/// 1. Load BankReconciliation aggregate by Id
/// 2. Call Reconcile(userId) domain method which validates state and applies reconciliation logic (matching cleared items, marking items reconciled)
/// 3. Persist changes via SaveChangesAsync
/// 4. Return Unit
/// 
/// Business Rules:
/// - Only reconciliations in Draft or Pending state may be approved
/// - Approval records ReconciledBy and ReconciledDate for audit
/// - GL posting or adjustment entries may be created by the domain method
/// 
/// Permissions: Requires authenticated user with reconciliation approval permission
/// 
/// Exceptions:
/// - NotFoundException: Thrown if reconciliation not found
/// - BusinessRuleException: Thrown if reconciliation is not in an approvable state
/// </remarks>
public class ApproveBankReconciliationHandler(AccountingDbContext context, ICurrentUser currentUser) 
    : ICommandHandler<ApproveBankReconciliationCommand>
{
    public async ValueTask<Unit> Handle(ApproveBankReconciliationCommand command, CancellationToken ct)
    {
        var entity = await context.BankReconciliations.FirstOrDefaultAsync(x => x.Id == command.Id, ct)
            ?? throw new NotFoundException("BankReconciliation not found");

        entity.Reconcile(currentUser.GetUserId());
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
