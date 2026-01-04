using FSH.Framework.Core.Exceptions;
using FSH.Modules.Accounting.Contracts.v1.InventoryItems;
using FSH.Modules.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Modules.Accounting.Features.v1.InventoryItems.GetInventoryItem;

public record GetInventoryItemQuery(Guid Id) : IQuery<InventoryItemDto>;

public class GetInventoryItemHandler(AccountingDbContext context) : IQueryHandler<GetInventoryItemQuery, InventoryItemDto>
{
    public async ValueTask<InventoryItemDto> Handle(GetInventoryItemQuery query, CancellationToken ct)
    {
        var entity = await context.InventoryItems
            .Where(x => x.Id == query.Id)
            .Select(x => new InventoryItemDto(
                x.Id,
                x.Name,
                x.Description,
                x.IsActive,
                x.CreatedOnUtc))
            .FirstOrDefaultAsync(ct);
        
        return entity ?? throw new NotFoundException("InventoryItem not found");
    }
}
