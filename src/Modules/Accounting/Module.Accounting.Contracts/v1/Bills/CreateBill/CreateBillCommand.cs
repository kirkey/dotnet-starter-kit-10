using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.Bills.CreateBill;

public record CreateBillCommand(string Name, string? Description) : ICommand<Guid>
{
}