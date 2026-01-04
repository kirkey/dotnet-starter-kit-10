// TODO: Implement Issue operation for Check
using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Contracts.v1.Checks.IssueCheck;
using FSH.Module.Accounting.Data;
using Mediator;

namespace FSH.Module.Accounting.Features.v1.Checks.IssueCheck;

/// <summary>
/// Handler for issuing a check - special operation for check workflow state transition.
/// </summary>
/// <remarks>
/// Responsibility: Transition check status from Draft/Printed to Issued state and record issuance details.
/// 
/// Execution Flow:
/// 1. Find check by Id using FindAsync(); throw NotFoundException if not found
/// 2. Call entity.Issue() domain method to perform state transition (Draft→Issued or Printed→Issued)
/// 3. Domain method validates current status and updates IssuedDate, IssuedBy tracking
/// 4. Persist changes to database via SaveChangesAsync
/// 5. Return Unit (void result)
/// 
/// State Transitions:
/// - Draft → Issued: Skips printing, issues check directly (rare use case)
/// - Printed → Issued: Transitions printed check to issued state (normal flow)
/// - Issued → Cleared: Requires bank reconciliation (handled by separate operations)
/// 
/// Business Rules:
/// - Check must be in Draft or Printed status before issuing
/// - Issued check cannot be updated (amount, payee locked)
/// - Issued checks require reversal/void instead of deletion
/// - GL posting occurs upon issuance (links to JournalEntryId)
/// 
/// Note: Current implementation is TODO - full workflow (GL posting, audit trail) needs completion.
/// 
/// Audit Trail: IssuedDate and IssuedBy recorded by domain method
/// GL Integration: May trigger posting to GL accounts (Accounts Payable or Bank Account)
/// 
/// Permissions: Requires authenticated user with check issuance permission
/// 
/// Exceptions:
/// - NotFoundException: Thrown if check with specified ID not found
/// - BusinessRuleException: Thrown if check not in issueable state (already issued/cleared/voided)
/// </remarks>
public class IssueCheckHandler(AccountingDbContext context) 
    : ICommandHandler<IssueCheckCommand>
{
    public async ValueTask<Unit> Handle(IssueCheckCommand command, CancellationToken ct)
    {
        var entity = await context.Checks.FindAsync(command.Id, ct) ?? throw new NotFoundException("Check not found");
        entity.Issue();
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
