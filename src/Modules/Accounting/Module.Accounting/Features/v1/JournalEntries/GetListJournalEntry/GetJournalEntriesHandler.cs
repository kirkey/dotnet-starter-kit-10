using FSH.Module.Accounting.Contracts.v1.JournalEntries;
using FSH.Module.Accounting.Contracts.v1.JournalEntries.GetListJournalEntry;
using FSH.Module.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Module.Accounting.Features.v1.JournalEntries.GetListJournalEntry;

/// <summary>
/// Get Journal Entries list query DTO.
/// 
/// **Purpose:**
/// Encapsulates the request to retrieve a paginated list of Journal Entries with extensive filtering.
/// 
/// **Parameters:**
/// - Page: Page number for pagination (default: 1)
/// - PageSize: Number of items per page (default: 10)
/// - SearchTerm: Text search across EntryNumber and ReferenceNumber
/// - IsActive: Filter by active status (null = all)
/// - Status: Filter by entry status (Draft, Posted, Approved, Reversed)
/// - EntryType: Filter by entry type (Manual, Automatic, Adjustment, Reversing, etc.)
/// - FromDate: Filter entries on or after this date (null = no minimum)
/// - ToDate: Filter entries on or before this date (null = no maximum)
/// 
/// **Multi-Tenancy:**
/// Tenant context is automatically applied via query filters.
/// 
/// **Filtering:**
/// Supports complex filtering combinations:
/// - Text search on EntryNumber and ReferenceNumber
/// - Status filtering (workflow states)
/// - EntryType classification filtering
/// - Date range filtering (inclusive)
/// - Active/Inactive status filtering
/// </summary>


/// <summary>
/// Handler for retrieving paginated list of Journal Entries.
/// 
/// **Responsibility:**
/// Queries the database for Journal Entries with multi-faceted filtering, sorting, and pagination.
/// Returns a summary DTO for each entry (lightweight representation).
/// 
/// **Execution Flow:**
/// 1. Build base queryable from DbSet
/// 2. Apply SearchTerm filter (EntryNumber OR ReferenceNumber contains)
/// 3. Apply IsActive filter (if specified)
/// 4. Apply Status filter (if specified - Draft/Posted/Approved/Reversed)
/// 5. Apply EntryType filter (if specified)
/// 6. Apply FromDate filter (if specified - greater than or equal)
/// 7. Apply ToDate filter (if specified - less than or equal)
/// 8. Count total matching records
/// 9. Sort by EntryDate descending (most recent first)
/// 10. Skip and take for pagination
/// 11. Project to summary DTOs
/// 12. Return paged response with items and metadata
/// 
/// **Filtering Logic:**
/// - SearchTerm: Case-sensitive substring match on EntryNumber or ReferenceNumber
/// - Status: Exact match (Draft, Posted, Approved, Reversed)
/// - EntryType: Exact match (Manual, Automatic, Adjustment, Reversing, etc.)
/// - Date Range: Inclusive (FromDate >= EntryDate <= ToDate)
/// - IsActive: Exact match on boolean flag
/// - Multiple filters: All specified filters applied (AND logic)
/// 
/// **Sorting:**
/// Primary: EntryDate (descending - most recent first)
/// 
/// **Pagination:**
/// - Page: 1-indexed
/// - PageSize: Records per page
/// - Skip calculation: (Page - 1) * PageSize
/// - TotalCount: Full count before pagination
/// 
/// **Returned Fields per Entry:**
/// - Id, EntryNumber, EntryDate
/// - EntryType, ReferenceNumber
/// - Status, TotalDebit, TotalCredit
/// - IsActive
/// 
/// **Permissions:**
/// Requires: Accounting.JournalEntry.View
/// 
/// **Business Rules:**
/// - Date filters are inclusive
/// - Status filter helps workflow management (find Posted entries awaiting approval, etc.)
/// - EntryType filter helps categorize entries by source (Manual vs Automatic adjustments)
/// </summary>
public class GetJournalEntriesHandler(AccountingDbContext context) 
    : IQueryHandler<FSH.Module.Accounting.Contracts.v1.JournalEntries.GetListJournalEntry.GetListJournalEntryQuery, FSH.Module.Accounting.Contracts.v1.JournalEntries.GetListJournalEntry.JournalEntriesPagedResponse>
{
    /// <summary>
    /// Handles the GetJournalEntriesQuery to retrieve a paginated, filtered list.
    /// </summary>
    /// <param name="query">The query containing pagination and multi-filter parameters</param>
    /// <param name="ct">Cancellation token for the operation</param>
    /// <returns>Paged response with journal entry summaries and total count</returns>
    public async ValueTask<JournalEntriesPagedResponse> Handle(GetListJournalEntryQuery query, CancellationToken ct)
    {
        var queryable = context.JournalEntries.AsQueryable();
        
        if (!string.IsNullOrWhiteSpace(query.SearchTerm))
        {
            queryable = queryable.Where(x => 
                x.EntryNumber.Contains(query.SearchTerm) ||
                x.ReferenceNumber.Contains(query.SearchTerm));
        }
        
        if (query.IsPosted.HasValue)
        {
            queryable = queryable.Where(x => x.IsPosted == query.IsPosted.Value);
        }
        
        var totalCount = await queryable.CountAsync(ct);
        
        var items = await queryable
            .OrderByDescending(x => x.EntryDate)
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .Select(x => new JournalEntrySummaryDto(
                x.Id,
                x.EntryNumber,
                x.EntryDate,
                x.EntryType,
                x.ReferenceNumber,
                x.TotalDebit,
                x.TotalCredit,
                x.Status,
                x.IsActive))
            .ToListAsync(ct);
        
        return new JournalEntriesPagedResponse(items, totalCount, query.Page, query.PageSize);
    }
}
