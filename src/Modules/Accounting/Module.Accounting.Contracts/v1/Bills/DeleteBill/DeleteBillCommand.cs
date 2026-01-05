using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.Bills.DeleteBill;

public record DeleteBillCommand(Guid Id) : ICommand
{
    public override bool Equals(object obj)
    {
        return Equals(obj as DeleteBillCommand);
    }
}