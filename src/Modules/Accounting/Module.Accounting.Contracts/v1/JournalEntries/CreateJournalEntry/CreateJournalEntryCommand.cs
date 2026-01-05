using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.JournalEntries.CreateJournalEntry;

/// <summary>
/// Create Journal Entry command.
/// </summary>
public record CreateJournalEntryCommand(
    string EntryNumber,
    DateTime EntryDate,
    string EntryType,
    string ReferenceNumber,
    Guid FiscalPeriodId,
    string? ReferenceType = null,
    string? Description = null,
    string? Notes = null,
    string? Memo = null) : ICommand<Guid>;