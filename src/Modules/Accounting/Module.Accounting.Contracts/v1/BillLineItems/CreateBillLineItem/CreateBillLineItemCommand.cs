using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.BillLineItems.CreateBillLineItem;

public record CreateBillLineItemCommand(string Name, string? Description) : ICommand<Guid>;