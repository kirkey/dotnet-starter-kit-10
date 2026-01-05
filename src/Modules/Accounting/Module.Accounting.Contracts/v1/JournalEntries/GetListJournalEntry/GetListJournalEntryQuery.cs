using FSH.Module.Accounting.Contracts.v1.JournalEntries;
using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.JournalEntries.GetListJournalEntry;

/// <summary>
/// Get Journal Entries list query.
/// </summary>
public record GetListJournalEntryQuery(
    int Page = 1,
    int PageSize = 10,
    string? SearchTerm = null,
    bool? IsPosted = null) : IQuery<JournalEntriesPagedResponse>;

/// <summary>
/// Response containing paginated Journal Entries list.
/// </summary>
public record JournalEntriesPagedResponse(
    List<JournalEntrySummaryDto> Items,
    int TotalCount,
    int Page,
    int PageSize);

