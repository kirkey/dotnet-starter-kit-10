using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.JournalEntries.ApproveJournalEntry;

/// <summary>
/// Approve Journal Entry command.
/// </summary>
public record ApproveJournalEntryCommand(Guid Id) : ICommand;