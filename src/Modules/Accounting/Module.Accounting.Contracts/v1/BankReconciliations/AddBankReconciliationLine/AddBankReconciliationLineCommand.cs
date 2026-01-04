namespace FSH.Module.Accounting.Contracts.v1.BankReconciliations;

public sealed record AddBankReconciliationLineCommand(
    Guid BankReconciliationId,
    Guid TransactionId,
    DateTime TransactionDate,
    decimal Amount,
    string? Description = null) : ICommand<Guid>;