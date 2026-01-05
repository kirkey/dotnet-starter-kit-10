namespace FSH.Module.Accounting.Contracts.v1.Bills.ApproveBill;

public record ApproveBillCommand(Guid Id) : ICommand
{
    public override bool Equals(object obj)
    {
        return Equals(obj as ApproveBillCommand);
    }
}