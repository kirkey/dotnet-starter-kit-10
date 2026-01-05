using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.JournalEntryLines.DeleteJournalEntryLine;

public sealed record DeleteJournalEntryLineCommand(Guid Id) : ICommand;
