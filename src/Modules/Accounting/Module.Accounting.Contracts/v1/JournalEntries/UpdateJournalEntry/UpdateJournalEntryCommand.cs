using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.JournalEntries.UpdateJournalEntry;

/// <summary>
/// Update Journal Entry command.
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