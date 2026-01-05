using FSH.Module.Accounting.Contracts.v1.InvoiceLineItems;
using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.InvoiceLineItems.GetListInvoiceLineItem;

public record GetInvoiceLineItemsQuery(
    int Page = 1,
    int PageSize = 10,
    string? SearchTerm = null,
    bool? IsActive = null) : IQuery<InvoiceLineItemsPagedResponse>;

public record InvoiceLineItemsPagedResponse(
    List<InvoiceLineItemSummaryDto> Items,
    int TotalCount,
    int Page,
    int PageSize);