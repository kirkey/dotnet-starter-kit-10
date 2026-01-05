using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Contracts.v1.JournalEntries;
using FSH.Module.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Module.Accounting.Features.v1.JournalEntries.GetJournalEntry;


/// <summary>
/// Handler for retrieving a single Journal Entry by ID.
/// 
/// **Responsibility:**
/// Queries the database for a Journal Entry by ID including all transaction details.
/// Returns complete entry with debit/credit totals and approval history.
/// 
/// **Execution Flow:**
/// 1. Query DbSet for JournalEntry by ID
/// 2. Project to JournalEntryDto with all entry properties
/// 3. Throw NotFoundException if not found
/// 4. Return complete DTO with all metadata
/// 
/// **Returned Fields:**
/// - Id, EntryNumber, EntryDate, EntryType
/// - ReferenceNumber, ReferenceType
/// - TotalDebit, TotalCredit (calculated from line items)
/// - FiscalPeriodId, Status (Draft, Posted, Approved, Reversed)
/// - PostedDate, PostedBy, ApprovedDate, ApprovedBy
/// - IsReversed, ReversedEntryId, ReversedDate
/// - Description, Notes, Memo
/// - IsActive, CreatedOnUtc
/// 
/// **Permissions:**
/// Requires: Accounting.JournalEntry.View
/// 
/// **Exceptions:**
/// - NotFoundException: Thrown when JournalEntry is not found
/// </summary>
public class GetJournalEntryHandler(AccountingDbContext context) : IQueryHandler<FSH.Module.Accounting.Contracts.v1.JournalEntries.GetJournalEntry.GetJournalEntryQuery, FSH.Module.Accounting.Contracts.v1.JournalEntries.JournalEntryDto>
{
    /// <summary>
    /// Handles the GetJournalEntryQuery to retrieve a Journal Entry.
    /// </summary>
    /// <param name="query">The query containing the Journal Entry ID to retrieve</param>
    /// <param name="ct">Cancellation token for the operation</param>
    /// <returns>The JournalEntryDto with complete entry details</returns>
    /// <exception cref="NotFoundException">Thrown when JournalEntry with the specified ID is not found</exception>
    public async ValueTask<FSH.Module.Accounting.Contracts.v1.JournalEntries.JournalEntryDto> Handle(FSH.Module.Accounting.Contracts.v1.JournalEntries.GetJournalEntry.GetJournalEntryQuery query, CancellationToken ct)
    {
        var entity = await context.JournalEntries
            .Where(x => x.Id == query.Id)
            .Select(x => new JournalEntryDto(
                x.Id,
                x.EntryNumber,
                x.EntryDate,
                x.EntryType,
                x.ReferenceNumber,
                x.ReferenceType,
                x.TotalDebit,
                x.TotalCredit,
                x.FiscalPeriodId,
                x.Status,
                x.PostedDate,
                x.PostedBy,
                x.ApprovedDate,
                x.ApprovedBy,
                x.IsReversed,
                x.ReversedEntryId,
                x.ReversedDate,
                x.Description,
                x.Notes,
                x.Memo,
                x.IsActive,
                x.CreatedOnUtc))
            .FirstOrDefaultAsync(ct);
        
        return entity ?? throw new NotFoundException("JournalEntry not found");
    }
}
