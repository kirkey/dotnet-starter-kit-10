using FSH.Module.Accounting.Contracts.v1.RecurringJournalEntries;
using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.RecurringJournalEntries.GetRecurringJournalEntry;

/// <summary>
/// Get Recurring Journal Entry query.
/// </summary>
/// <param name="Id">Recurring journal entry ID to retrieve</param>
public record GetRecurringJournalEntryQuery(Guid Id) : IQuery<RecurringJournalEntryDto>;