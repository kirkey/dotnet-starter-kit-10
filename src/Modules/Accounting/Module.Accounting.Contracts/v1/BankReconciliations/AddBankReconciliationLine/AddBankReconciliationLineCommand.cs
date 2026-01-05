namespace FSH.Module.Accounting.Contracts.v1.BankReconciliations;

public sealed record AddBankReconciliationLineCommand(
    Guid BankReconciliationId,
    Guid TransactionId,
    DateTime TransactionDate,
    decimal Amount,
    string? Description = null) : ICommand<Guid>
{
    public override bool Equals(object obj)
    {
        return Equals(obj as AddBankReconciliationLineCommand);
    }
}