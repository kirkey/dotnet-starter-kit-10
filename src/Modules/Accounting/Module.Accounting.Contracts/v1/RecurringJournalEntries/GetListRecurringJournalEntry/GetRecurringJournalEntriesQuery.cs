using Mediator;
using FSH.Module.Accounting.Contracts.v1.RecurringJournalEntries;

namespace FSH.Module.Accounting.Contracts.v1.RecurringJournalEntries.GetListRecurringJournalEntry;

/// <summary>
/// Get Recurring Journal Entries (paginated) query.
/// </summary>
/// <param name="Page">Page number for pagination (1-based, default=1)</param>
/// <param name="PageSize">Number of items per page (default=10)</param>
/// <param name="SearchTerm">Optional filter by name (contains search)</param>
/// <param name="IsActive">Optional filter by active status</param>
/// <param name="Frequency">Optional filter by frequency</param>
public record GetRecurringJournalEntriesQuery(
    int Page = 1,
    int PageSize = 10,
    string? SearchTerm = null,
    bool? IsActive = null,
    string? Frequency = null) : IQuery<RecurringJournalEntriesPagedResponse>;

/// <summary>
/// Response object for paginated recurring journal entry list.
/// </summary>
/// <param name="Items">List of RecurringJournalEntrySummaryDto</param>
/// <param name="TotalCount">Total count of entries matching filters</param>
/// <param name="Page">Requested page number</param>
/// <param name="PageSize">Items per page</param>
public record RecurringJournalEntriesPagedResponse(
    List<RecurringJournalEntrySummaryDto> Items,
    int TotalCount,
    int Page,
    int PageSize);