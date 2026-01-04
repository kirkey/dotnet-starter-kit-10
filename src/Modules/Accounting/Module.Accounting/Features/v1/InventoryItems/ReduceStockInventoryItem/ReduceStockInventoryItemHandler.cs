using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Contracts.v1.InventoryItems.ReduceStockInventoryItem;
using FSH.Module.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Module.Accounting.Features.v1.InventoryItems.ReduceStockInventoryItem;

public class ReduceStockInventoryItemHandler(AccountingDbContext context) 
    : ICommandHandler<ReduceStockInventoryItemCommand>
{
    public async ValueTask<Unit> Handle(ReduceStockInventoryItemCommand command, CancellationToken ct)
    {
        var entity = await context.InventoryItems.FirstOrDefaultAsync(x => x.Id == command.Id, ct)
            ?? throw new NotFoundException("InventoryItem not found");

        entity.ReduceStock(command.Quantity);
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
