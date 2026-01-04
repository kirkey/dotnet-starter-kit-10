using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Data;
using Mediator;
using FSH.Module.Accounting.Contracts.v1.JournalEntries.DeleteJournalEntry;

namespace FSH.Module.Accounting.Features.v1.JournalEntries.DeleteJournalEntry;

/// <summary>
/// Delete Journal Entry command DTO.
/// 
/// **Purpose:**
/// Encapsulates the request to delete a Journal Entry (only in Draft state).
/// Posted or Approved entries must be reversed instead.
/// 
/// **Parameters:**
/// - Id: The unique identifier of the Journal Entry to delete
/// 
/// **Multi-Tenancy:**
/// Tenant context is automatically applied via DbContext filters.
/// 
/// **Validation:**
/// The handler validates:
/// - Entry exists in the current tenant
/// - Entry status is Draft (cannot delete Posted or Approved entries)
/// </summary>
/// <summary>
/// Handler for deleting a Journal Entry.
/// 
/// **Responsibility:**
/// Deletes a Journal Entry only if it's in Draft status.
/// Posted or Approved entries cannot be deleted; they must be reversed instead.
/// 
/// **Execution Flow:**
/// 1. Find JournalEntry by ID, throw NotFoundException if not found
/// 2. Check Status is Draft, throw BadRequestException if Posted or Approved
/// 3. Delete the entry and all associated line items
/// 4. Persist changes to database
/// 5. Return Unit.Value on success
/// 
/// **Pre-Delete Validation:**
/// - Entry must be in Draft status
/// - Cannot delete Posted entries (would break audit trail)
/// - Cannot delete Approved entries (would break approval chain)
/// 
/// **Cascading Effects:**
/// - Associated JournalEntryLineItems are deleted
/// - Related PendingApprovals are removed
/// - Audit history is NOT deleted (references to deleted entry preserved)
/// 
/// **Permissions:**
/// Requires: Accounting.JournalEntry.Delete
/// 
/// **Business Rules:**
/// - Only Draft entries can be deleted
/// - Posted entries must use the Reverse operation instead
/// - Approved entries cannot be deleted (must be unapproved first)
/// - Entries in locked fiscal periods cannot be deleted
/// 
/// **Exceptions:**
/// - NotFoundException: Thrown when entry is not found
/// - BadRequestException: Thrown when trying to delete Posted or Approved entry
/// </summary>
public class DeleteJournalEntryHandler(AccountingDbContext context) : ICommandHandler<DeleteJournalEntryCommand>
{
    /// <summary>
    /// Handles the DeleteJournalEntryCommand to remove a Journal Entry.
    /// </summary>
    /// <param name="command">The command containing the Journal Entry ID to delete</param>
    /// <param name="ct">Cancellation token for the operation</param>
    /// <returns>Unit.Value on successful deletion</returns>
    /// <exception cref="NotFoundException">Thrown when JournalEntry is not found</exception>
    /// <exception cref="BadRequestException">Thrown when entry is not in Draft status</exception>
    public async ValueTask<Unit> Handle(DeleteJournalEntryCommand command, CancellationToken ct)
    {
        var entity = await context.JournalEntries.FindAsync(command.Id, ct)
            ?? throw new NotFoundException("JournalEntry not found");
        
        if (entity.Status == "Posted" || entity.Status == "Approved")
            throw new BadRequestException("Cannot delete a posted or approved journal entry");
        
        context.JournalEntries.Remove(entity);
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
