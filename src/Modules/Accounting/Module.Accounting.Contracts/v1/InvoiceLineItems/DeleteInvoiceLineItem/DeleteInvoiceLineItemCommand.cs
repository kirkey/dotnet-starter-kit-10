using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.InvoiceLineItems.DeleteInvoiceLineItem;

public record DeleteInvoiceLineItemCommand(Guid Id) : ICommand;