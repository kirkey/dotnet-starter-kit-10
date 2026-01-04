using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.RecurringJournalEntries.GenerateRecurringJournalEntry;

/// <summary>
/// Generate Recurring Journal Entry command - generates a journal entry instance from the recurring template.
/// </summary>
/// <param name="Id">Recurring journal entry template ID to generate from</param>
public record GenerateRecurringJournalEntryCommand(Guid Id) : ICommand;
