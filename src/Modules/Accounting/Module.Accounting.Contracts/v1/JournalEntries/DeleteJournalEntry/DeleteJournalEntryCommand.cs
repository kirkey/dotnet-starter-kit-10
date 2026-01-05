using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.JournalEntries.DeleteJournalEntry;

/// <summary>
/// Delete Journal Entry command.
/// </summary>
public record DeleteJournalEntryCommand(Guid Id) : ICommand;