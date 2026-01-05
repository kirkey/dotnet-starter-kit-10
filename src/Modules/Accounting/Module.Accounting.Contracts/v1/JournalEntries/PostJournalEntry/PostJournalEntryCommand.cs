using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.JournalEntries.PostJournalEntry;

/// <summary>
/// Post Journal Entry command.
/// </summary>
public record PostJournalEntryCommand(Guid Id) : ICommand;