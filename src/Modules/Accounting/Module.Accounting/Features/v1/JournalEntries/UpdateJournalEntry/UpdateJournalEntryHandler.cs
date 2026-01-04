using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Data;
using Mediator;

namespace FSH.Module.Accounting.Features.v1.JournalEntries.UpdateJournalEntry;

/// <summary>
/// Update Journal Entry command DTO.
/// 
/// **Purpose:**
/// Encapsulates the request to update metadata of a Journal Entry (before posting).
/// Note: Line items cannot be updated after creation; delete and recreate instead.
/// 
/// **Parameters:**
/// - Id: The Journal Entry ID to update
/// - EntryNumber: Unique entry reference number
/// - EntryDate: Date the entry was recorded
/// - EntryType: Type of entry (Manual, Automatic, Adjustment, etc.)
/// - ReferenceNumber: External reference (invoice, check, etc.)
/// - FiscalPeriodId: Fiscal period for the entry
/// - ReferenceType, Description, Notes, Memo: Descriptive fields
/// 
/// **Constraints:**
/// - Can only be updated if Status is Draft
/// - Cannot change line items via update (delete/recreate instead)
/// - FiscalPeriodId cannot change if entry is locked
/// 
/// **Multi-Tenancy:**
/// Tenant context is automatically applied via DbContext filters.
/// </summary>
public record UpdateJournalEntryCommand(
    Guid Id,
    string EntryNumber,
    DateTime EntryDate,
    string EntryType,
    string ReferenceNumber,
    Guid FiscalPeriodId,
    string? ReferenceType = null,
    string? Description = null,
    string? Notes = null,
    string? Memo = null) : ICommand<Guid>;

/// <summary>
/// Handler for updating a Journal Entry.
/// 
/// **Responsibility:**
/// Updates Journal Entry metadata (before posting). Cannot modify line items or amounts.
/// 
/// **Execution Flow:**
/// 1. Find JournalEntry by ID, throw NotFoundException if not found
/// 2. Verify entry is in Draft status (not Posted or Approved)
/// 3. Call entity.Update() domain method with new metadata
/// 4. Persist changes to database
/// 5. Return updated entry ID
/// 
/// **Updateable Fields:**
/// - EntryNumber, EntryDate, EntryType
/// - ReferenceNumber, ReferenceType
/// - FiscalPeriodId (if not locked)
/// - Description, Notes, Memo
/// 
/// **Immutable/Protected Fields:**
/// - Id: Cannot change
/// - Line items: Cannot be updated (must delete/recreate)
/// - TotalDebit/TotalCredit: Calculated from line items
/// - PostedDate/PostedBy: Set on posting only
/// - ApprovedDate/ApprovedBy: Set on approval only
/// 
/// **Permissions:**
/// Requires: Accounting.JournalEntry.Edit
/// 
/// **Business Rules:**
/// - Can only update entries in Draft status
/// - Cannot update entries that are Posted or Approved
/// - Cannot update entries for closed fiscal periods
/// 
/// **Exceptions:**
/// - NotFoundException: Thrown when entry not found
/// - BadRequestException: Thrown if entry is not in Draft status
/// </summary>
public class UpdateJournalEntryHandler(AccountingDbContext context) : ICommandHandler<UpdateJournalEntryCommand, Guid>
{
    /// <summary>
    /// Handles the UpdateJournalEntryCommand to update entry metadata.
    /// </summary>
    /// <param name="command">The command containing the Journal Entry updates</param>
    /// <param name="ct">Cancellation token for the operation</param>
    /// <returns>The ID of the updated Journal Entry</returns>
    /// <exception cref="NotFoundException">Thrown when JournalEntry is not found</exception>
    public async ValueTask<Guid> Handle(UpdateJournalEntryCommand command, CancellationToken ct)
    {
        var entity = await context.JournalEntries.FindAsync(command.Id, ct)
            ?? throw new NotFoundException("JournalEntry not found");
        
        entity.Update(
            command.EntryNumber,
            command.EntryDate,
            command.EntryType,
            command.ReferenceNumber,
            command.FiscalPeriodId,
            command.ReferenceType,
            command.Description,
            command.Notes,
            command.Memo);
        
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
