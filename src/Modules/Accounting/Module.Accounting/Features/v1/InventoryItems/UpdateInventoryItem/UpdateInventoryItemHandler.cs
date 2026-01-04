using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Data;
using Mediator;

namespace FSH.Module.Accounting.Features.v1.InventoryItems.UpdateInventoryItem;

public record UpdateInventoryItemCommand(Guid Id, string Name, string? Description) : ICommand<Guid>;

public class UpdateInventoryItemHandler(AccountingDbContext context) : ICommandHandler<UpdateInventoryItemCommand, Guid>
{
    public async ValueTask<Guid> Handle(UpdateInventoryItemCommand command, CancellationToken ct)
    {
        var entity = await context.InventoryItems.FindAsync(command.Id, ct)
            ?? throw new NotFoundException("InventoryItem not found");
        
        entity.Update(command.Name, command.Description);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
