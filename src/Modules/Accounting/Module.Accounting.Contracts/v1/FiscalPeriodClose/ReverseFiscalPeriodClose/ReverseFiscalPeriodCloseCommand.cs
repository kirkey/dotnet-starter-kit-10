using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.FiscalPeriodClose.ReverseFiscalPeriodClose;

public sealed record ReverseFiscalPeriodCloseCommand(Guid Id) : ICommand;