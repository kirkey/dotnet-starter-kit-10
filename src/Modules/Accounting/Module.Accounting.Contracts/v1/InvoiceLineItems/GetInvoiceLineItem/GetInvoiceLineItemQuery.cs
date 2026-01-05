using FSH.Module.Accounting.Contracts.v1.InvoiceLineItems;
using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.InvoiceLineItems.GetInvoiceLineItem;

public record GetInvoiceLineItemQuery(Guid Id) : IQuery<InvoiceLineItemDto>;