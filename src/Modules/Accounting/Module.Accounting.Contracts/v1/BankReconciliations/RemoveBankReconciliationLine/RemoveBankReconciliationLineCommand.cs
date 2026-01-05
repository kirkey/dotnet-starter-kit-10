using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.BankReconciliations.RemoveBankReconciliationLine;

public sealed record RemoveBankReconciliationLineCommand(Guid BankReconciliationLineId) : ICommand
{
}