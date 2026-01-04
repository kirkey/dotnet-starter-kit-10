using FSH.Framework.Core.Context;
using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Data;
using FSH.Module.Accounting.Domain;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Module.Accounting.Features.v1.JournalEntries.ApproveJournalEntry;

/// <summary>
/// Approve Journal Entry command DTO.
/// 
/// **Purpose:**
/// Encapsulates the request to approve a Journal Entry, transitioning it from Posted to Approved status.
/// Approval indicates the entry has been reviewed and authorized for final processing.
/// 
/// **Parameters:**
/// - Id: The Journal Entry ID to approve
/// 
/// **Business Constraints:**
/// - Entry must be in Posted status (must be posted first)
/// - Requires approval authority for the fiscal period
/// - Approval is typically required before GL posting finalizes
/// 
/// **Multi-Tenancy:**
/// Tenant context is automatically applied via DbContext filters.
/// </summary>
public record ApproveJournalEntryCommand(Guid Id) : ICommand;

/// <summary>
/// Handler for approving a Journal Entry.
/// 
/// **Responsibility:**
/// Approves a Journal Entry, transitioning it from Posted to Approved status.
/// Records approval metadata and may trigger downstream processing.
/// 
/// **Execution Flow:**
/// 1. Find JournalEntry by ID
/// 2. Verify entry is in Posted status
/// 3. Call entity.Approve() domain method
/// 4. Record ApprovedDate and ApprovedBy information
/// 5. Persist changes to database
/// 6. Return Unit.Value on success
/// 
/// **Approval Process:**
/// - Sets ApprovedDate to current UTC time
/// - Records ApprovedBy user ID
/// - Updates entry Status to "Approved"
/// - May trigger notification to accounting staff
/// - May enable further processing (bank reconciliation, period close, etc.)
/// 
/// **Permissions:**
/// Requires: Accounting.JournalEntry.Approve
/// (Typically restricted to managers or senior accounting staff)
/// 
/// **Business Rules:**
/// - Can only approve Posted entries
/// - Cannot approve Draft entries (must post first)
/// - Cannot unapprove entries (new approval process required for changes)
/// - Approval may trigger GL subledger finalization
/// - May enforce approval hierarchies (manager level, amount thresholds)
/// 
/// **Exceptions:**
/// - NotFoundException: Thrown when entry not found
/// - BadRequestException: Thrown if entry is not in Posted status
/// </summary>
public class ApproveJournalEntryHandler(AccountingDbContext context, ICurrentUser currentUser) 
    : ICommandHandler<ApproveJournalEntryCommand>
{
    /// <summary>
    /// Handles the ApproveJournalEntryCommand to approve a Journal Entry.
    /// </summary>
    /// <param name="command">The command containing the Journal Entry ID to approve</param>
    /// <param name="ct">Cancellation token for the operation</param>
    /// <returns>Unit.Value on successful approval</returns>
    /// <exception cref="NotFoundException">Thrown when JournalEntry is not found</exception>
    /// <exception cref="BadRequestException">Thrown if entry is not in Posted status</exception>
    public async ValueTask<Unit> Handle(ApproveJournalEntryCommand command, CancellationToken ct)
    {
        var entity = await context.JournalEntries
            .FirstOrDefaultAsync(x => x.Id == command.Id, ct)
            ?? throw new NotFoundException("JournalEntry not found");

        if (entity.Status != "Posted")
            throw new BadRequestException("Journal entry must be posted before approval");

        entity.Approve(currentUser.GetUserId(), DateTime.UtcNow);

        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
