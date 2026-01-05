using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.Bills.UpdateBill;

public record UpdateBillCommand(Guid Id, string Name, string? Description) : ICommand<Guid>
{
    public override bool Equals(object obj)
    {
        return Equals(obj as UpdateBillCommand);
    }
}