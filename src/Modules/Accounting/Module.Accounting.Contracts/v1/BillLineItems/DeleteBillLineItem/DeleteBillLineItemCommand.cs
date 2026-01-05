using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.BillLineItems.DeleteBillLineItem;

public record DeleteBillLineItemCommand(Guid Id) : ICommand;