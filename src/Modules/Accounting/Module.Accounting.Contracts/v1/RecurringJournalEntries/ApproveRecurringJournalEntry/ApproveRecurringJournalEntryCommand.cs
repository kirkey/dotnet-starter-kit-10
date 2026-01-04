using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.RecurringJournalEntries.ApproveRecurringJournalEntry;

/// <summary>
/// Approve Recurring Journal Entry command.
/// </summary>
/// <param name="Id">Recurring journal entry ID to approve</param>
public record ApproveRecurringJournalEntryCommand(Guid Id) : ICommand;
