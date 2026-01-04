namespace FSH.Module.Accounting.Contracts.v1.BankReconciliations;

public sealed record RemoveBankReconciliationLineCommand(Guid BankReconciliationLineId) : ICommand;