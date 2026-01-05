using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.FiscalPeriodClose.InitiateFiscalPeriodClose;

public sealed record InitiateFiscalPeriodCloseCommand(Guid Id) : ICommand;