using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Module.Accounting.Features.v1.InventoryItems.AddStockInventoryItem;

public record AddStockInventoryItemCommand(Guid Id, decimal Quantity) : ICommand;

public class AddStockInventoryItemHandler(AccountingDbContext context) 
    : ICommandHandler<AddStockInventoryItemCommand>
{
    public async ValueTask<Unit> Handle(AddStockInventoryItemCommand command, CancellationToken ct)
    {
        var entity = await context.InventoryItems.FirstOrDefaultAsync(x => x.Id == command.Id, ct)
            ?? throw new NotFoundException("InventoryItem not found");

        entity.AddStock(command.Quantity);
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
