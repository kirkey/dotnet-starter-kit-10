using FSH.Framework.Core.Exceptions;
using FSH.Modules.Accounting.Contracts.v1.BillLineItems;
using FSH.Modules.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Modules.Accounting.Features.v1.BillLineItems.GetBillLineItem;

public record GetBillLineItemQuery(Guid Id) : IQuery<BillLineItemDto>;

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
