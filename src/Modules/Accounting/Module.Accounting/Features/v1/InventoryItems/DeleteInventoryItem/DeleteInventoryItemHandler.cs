using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Data;
using Mediator;

namespace FSH.Module.Accounting.Features.v1.InventoryItems.DeleteInventoryItem;

public record DeleteInventoryItemCommand(Guid Id) : ICommand;

public class DeleteInventoryItemHandler(AccountingDbContext context) : ICommandHandler<DeleteInventoryItemCommand>
{
    public async ValueTask<Unit> Handle(DeleteInventoryItemCommand command, CancellationToken ct)
    {
        var entity = await context.InventoryItems.FindAsync(command.Id, ct)
            ?? throw new NotFoundException("InventoryItem not found");
        
        context.InventoryItems.Remove(entity);
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
