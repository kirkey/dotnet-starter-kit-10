using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.Bills.UpdateBill;

public record UpdateBillCommand(Guid Id, string Name, string? Description) : ICommand<Guid>
{
}