using FSH.Module.Accounting.Contracts.v1.JournalEntryLines;
using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.JournalEntryLines.GetJournalEntryLine;

public record GetJournalEntryLineQuery(Guid Id) : IQuery<JournalEntryLineDto>;
