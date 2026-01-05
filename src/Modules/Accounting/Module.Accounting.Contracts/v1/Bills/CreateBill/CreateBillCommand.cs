using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.Bills.CreateBill;

public record CreateBillCommand(string Name, string? Description) : ICommand<Guid>
{
    public override bool Equals(object obj)
    {
        return Equals(obj as CreateBillCommand);
    }
}