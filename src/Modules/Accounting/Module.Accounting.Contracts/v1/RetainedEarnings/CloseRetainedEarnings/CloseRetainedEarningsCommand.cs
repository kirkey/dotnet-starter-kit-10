using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.RetainedEarnings.CloseRetainedEarnings;

public sealed record CloseRetainedEarningsCommand(Guid Id, int FiscalYear, decimal ClosingBalance) : ICommand;