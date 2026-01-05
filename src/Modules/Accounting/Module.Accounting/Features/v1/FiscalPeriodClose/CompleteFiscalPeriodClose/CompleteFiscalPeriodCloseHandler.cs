using FSH.Framework.Core.Exceptions;
using FSH.Framework.Shared.Identity;
using FSH.Module.Accounting.Data;
using Mediator;
using FSH.Module.Accounting.Contracts.v1.FiscalPeriodClose.CompleteFiscalPeriodClose;

namespace FSH.Module.Accounting.Features.v1.FiscalPeriodClose.CompleteFiscalPeriodClose;

/// <summary>
/// Command to complete a fiscal period close and record the closing journal entry reference.
/// </summary>
/// <param name="Id">FiscalPeriodClose ID to complete</param>
/// <param name="ClosingJournalEntryId">Journal entry Id for the closing/retained earnings entry</param> 

/// <summary>
/// Handler for completing the fiscal period close. The operation finalizes the period and links the closing journal entry.
/// </summary>
/// <remarks>
/// Responsibility: Call CompleteClose(userId, closingJournalEntryId) domain method to perform final postings and set period state to Completed.
/// 
/// Execution Flow:
/// 1. Load FiscalPeriodClose aggregate by Id
/// 2. Call CompleteClose(currentUserId, ClosingJournalEntryId) which performs GL postings and status transition
/// 3. Persist changes via SaveChangesAsync
/// 
/// Business Rules:
/// - Only initiated periods may be completed
/// - ClosingJournalEntryId must reference a valid JournalEntry that posts retained earnings adjustments
/// - Completion should lock the period to prevent further transactions
/// 
/// Permissions: Requires FiscalPeriodClose.Complete
/// 
/// Exceptions:
/// - NotFoundException: Thrown if FiscalPeriodClose not found
/// - BusinessRuleException: Thrown if period is not in an appropriate state
/// </remarks>
public class CompleteFiscalPeriodCloseHandler(AccountingDbContext context, ICurrentUser currentUser) 
    : ICommandHandler<CompleteFiscalPeriodCloseCommand>
{
    public async ValueTask<Unit> Handle(CompleteFiscalPeriodCloseCommand command, CancellationToken ct)
    {
        var entity = await context.FiscalPeriodClose.FindAsync(command.Id, ct) ?? throw new NotFoundException("FiscalPeriodClose not found");
        entity.CompleteClose(currentUser.GetUserId(), command.ClosingJournalEntryId);
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
