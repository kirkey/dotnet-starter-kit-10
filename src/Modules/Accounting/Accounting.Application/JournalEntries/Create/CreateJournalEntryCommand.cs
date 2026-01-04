namespace Accounting.Application.JournalEntries.Create;

/// <summary>
/// Command to create a new Journal Entry.
/// </summary>
/// <param name>Effective date of the journal entry.</param>
/// <param name>External reference or document number.</param>
/// <param name>Source system or module that created the entry.</param>
/// <param name>Description of the journal entry.</param>
/// <param name>Collection of journal entry line items (debits and credits).</param>
/// <param name>Optional accounting period identifier.</param>
/// <param name>Original amount for reference purposes.</param>
/// <param name>Optional notes.</param>
public sealed record CreateJournalEntryCommand(
    DateTime Date,
    string ReferenceNumber,
    string Source,
    string Description,
    List<JournalEntryLineDto>? Lines,
    DefaultIdType? PeriodId,
    decimal OriginalAmount,
    string? Notes
) : IRequest<CreateJournalEntryResponse>;

