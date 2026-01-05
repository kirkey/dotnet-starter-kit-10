using FSH.Module.Accounting.Contracts.v1.BillLineItems;
using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.BillLineItems.GetBillLineItem;

public record GetBillLineItemQuery(Guid Id) : IQuery<BillLineItemDto>;