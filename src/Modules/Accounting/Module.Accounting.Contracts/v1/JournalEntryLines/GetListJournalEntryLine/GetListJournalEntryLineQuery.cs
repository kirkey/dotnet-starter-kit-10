using FSH.Module.Accounting.Contracts.v1.JournalEntryLines;
using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.JournalEntryLines.GetListJournalEntryLine;

public record GetListJournalEntryLineQuery(
    int Page = 1,
    int PageSize = 10,
    string? SearchTerm = null,
    bool? IsActive = null,
    Guid? JournalEntryId = null) : IQuery<JournalEntryLinesPagedResponse>;

public record JournalEntryLinesPagedResponse(
    List<JournalEntryLineSummaryDto> Items,
    int TotalCount,
    int Page,
    int PageSize);
