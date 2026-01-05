using FSH.Module.Accounting.Contracts.v1.BillLineItems;
using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.BillLineItems.GetListBillLineItem;

public record GetBillLineItemsQuery(
    int Page = 1,
    int PageSize = 10,
    string? SearchTerm = null,
    bool? IsActive = null) : IQuery<BillLineItemsPagedResponse>;

public record BillLineItemsPagedResponse(
    List<BillLineItemSummaryDto> Items,
    int TotalCount,
    int Page,
    int PageSize);