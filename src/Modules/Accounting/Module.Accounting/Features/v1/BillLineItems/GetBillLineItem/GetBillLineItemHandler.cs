using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Contracts.v1.BillLineItems;
using FSH.Module.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;
using FSH.Module.Accounting.Contracts.v1.BillLineItems.GetBillLineItem;

namespace FSH.Module.Accounting.Features.v1.BillLineItems.GetBillLineItem;

public class GetBillLineItemHandler(AccountingDbContext context) : IQueryHandler<GetBillLineItemQuery, BillLineItemDto>
{
    public async ValueTask<BillLineItemDto> Handle(GetBillLineItemQuery query, CancellationToken ct)
    {
        var entity = await context.BillLineItems
            .Where(x => x.Id == query.Id)
            .Select(x => new BillLineItemDto(
                x.Id,
                x.Name,
                x.Description,
                x.IsActive,
                x.CreatedOnUtc))
            .FirstOrDefaultAsync(ct);
        
        return entity ?? throw new NotFoundException("BillLineItem not found");
    }
}
