using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.JournalEntries.ReverseJournalEntry;

/// <summary>
/// Reverse Journal Entry command.
/// </summary>
public record ReverseJournalEntryCommand(Guid Id, string? ReversalReason = null) : ICommand;
