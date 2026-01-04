namespace Accounting.Application.RecurringJournalEntries.Update.v1;

public sealed record UpdateRecurringJournalEntryCommand(
    DefaultIdType Id,
    string? Description,
    decimal? Amount,
    DateTime? EndDate,
    string? Memo,
    string? Notes
) : IRequest<DefaultIdType>;
