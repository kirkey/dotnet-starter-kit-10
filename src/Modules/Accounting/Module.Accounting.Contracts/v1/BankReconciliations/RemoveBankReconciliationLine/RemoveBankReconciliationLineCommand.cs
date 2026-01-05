namespace FSH.Module.Accounting.Contracts.v1.BankReconciliations;

public sealed record RemoveBankReconciliationLineCommand(Guid BankReconciliationLineId) : ICommand
{
    public override bool Equals(object obj)
    {
        return Equals(obj as RemoveBankReconciliationLineCommand);
    }
}