using FSH.Module.Accounting.Contracts.v1.JournalEntries;
using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.JournalEntries.GetJournalEntry;

/// <summary>
/// Get Journal Entry query.
/// </summary>
public record GetJournalEntryQuery(Guid Id) : IQuery<JournalEntryDto>;
