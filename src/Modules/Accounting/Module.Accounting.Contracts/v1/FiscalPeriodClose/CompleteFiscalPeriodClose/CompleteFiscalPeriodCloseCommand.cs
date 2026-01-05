using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.FiscalPeriodClose.CompleteFiscalPeriodClose;

public sealed record CompleteFiscalPeriodCloseCommand(Guid Id, Guid ClosingJournalEntryId) : ICommand;