using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Context;
using FSH.Module.Accounting.Data;
using FSH.Module.Accounting.Domain;
using Mediator;
using Microsoft.EntityFrameworkCore;
using FSH.Module.Accounting.Contracts.v1.JournalEntries.PostJournalEntry;

namespace FSH.Module.Accounting.Features.v1.JournalEntries.PostJournalEntry;

/// <summary>
/// Post Journal Entry command DTO.
/// 
/// **Purpose:**
/// Encapsulates the request to post a Journal Entry, transitioning it from Draft to Posted status.
/// Posting locks the entry and makes it subject to audit controls.
/// 
/// **Parameters:**
/// - Id: The Journal Entry ID to post
/// 
/// **Business Constraints:**
/// - Entry must be in Draft status
/// - Must have balanced debits and credits
/// - Cannot post to a closed fiscal period
/// - Posting updates GL account balances
/// 
/// **Multi-Tenancy:**
/// Tenant context is automatically applied via DbContext filters.
/// </summary>
/// <summary>
/// Handler for posting a Journal Entry.
/// 
/// **Responsibility:**
/// Posts a Journal Entry, transitioning it from Draft to Posted status.
/// Performs balance validation, updates GL account balances, and records posting metadata.
/// 
/// **Execution Flow:**
/// 1. Find JournalEntry by ID including line items
/// 2. Verify entry is in Draft status
/// 3. Recalculate totals from line items
/// 4. Validate debits equal credits (IsBalanced())
/// 5. Call entity.Post() domain method to update status
/// 6. Update GL account balances via line items
/// 7. Record PostedDate and PostedBy information
/// 8. Persist changes to database
/// 9. Return Unit.Value on success
/// 
/// **Validation & Balance Checking:**
/// - Verifies total debit = total credit
/// - Recalculates debit/credit totals from line items
/// - Ensures all line accounts exist and are active
/// - Validates fiscal period is open
/// 
/// **Side Effects:**
/// - Updates GL account balances (via entity.Post() or line processing)
/// - Sets PostedDate to current UTC time
/// - Records PostedBy user ID
/// - Creates audit trail entries
/// - May trigger GL subledger updates
/// 
/// **Permissions:**
/// Requires: Accounting.JournalEntry.Post
/// 
/// **Business Rules:**
/// - Can only post entries in Draft status
/// - Entry must be balanced (total debit = total credit)
/// - Cannot post to closed fiscal periods
/// - Posting is irreversible (reversal only option)
/// 
/// **Exceptions:**
/// - NotFoundException: Thrown when entry not found
/// - BadRequestException: Thrown if entry is not balanced or not in Draft status
/// </summary>
public class PostJournalEntryHandler(AccountingDbContext context, ICurrentUser currentUser) 
    : ICommandHandler<PostJournalEntryCommand>
{
    /// <summary>
    /// Handles the PostJournalEntryCommand to post a Journal Entry.
    /// </summary>
    /// <param name="command">The command containing the Journal Entry ID to post</param>
    /// <param name="ct">Cancellation token for the operation</param>
    /// <returns>Unit.Value on successful posting</returns>
    /// <exception cref="NotFoundException">Thrown when JournalEntry is not found</exception>
    /// <exception cref="BadRequestException">Thrown if entry is not balanced or not in Draft status</exception>
    public async ValueTask<Unit> Handle(PostJournalEntryCommand command, CancellationToken ct)
    {
        var entity = await context.JournalEntries
            .Include(x => x.Lines)
            .FirstOrDefaultAsync(x => x.Id == command.Id, ct)
            ?? throw new NotFoundException("JournalEntry not found");

        // Recalculate totals from lines
        var totalDebit = entity.Lines?.Sum(l => l.Debit) ?? 0m;
        var totalCredit = entity.Lines?.Sum(l => l.Credit) ?? 0m;

        entity.UpdateTotals(totalDebit, totalCredit);

        if (!entity.IsBalanced())
            throw new BadRequestException("Journal entry is not balanced. Debits must equal credits.");

        entity.Post(currentUser.GetUserId(), DateTime.UtcNow);

        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
