using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.BillLineItems.UpdateBillLineItem;

public record UpdateBillLineItemCommand(Guid Id, string Name, string? Description) : ICommand<Guid>;