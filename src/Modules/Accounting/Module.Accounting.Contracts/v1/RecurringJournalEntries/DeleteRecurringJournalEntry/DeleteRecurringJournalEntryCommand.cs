using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.RecurringJournalEntries.DeleteRecurringJournalEntry;

/// <summary>
/// Delete Recurring Journal Entry command.
/// </summary>
/// <param name="Id">Recurring journal entry ID to delete</param>
public record DeleteRecurringJournalEntryCommand(Guid Id) : ICommand;