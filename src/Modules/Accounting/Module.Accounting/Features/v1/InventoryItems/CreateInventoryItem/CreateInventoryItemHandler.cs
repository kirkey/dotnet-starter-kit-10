using FSH.Framework.Shared.Identity;
using FSH.Module.Accounting.Contracts.v1.InventoryItems.CreateInventoryItem;
using FSH.Module.Accounting.Data;
using FSH.Module.Accounting.Domain;
using Mediator;

namespace FSH.Module.Accounting.Features.v1.InventoryItems.CreateInventoryItem;

public class CreateInventoryItemHandler(AccountingDbContext context, ICurrentUser currentUser) 
    : ICommandHandler<CreateInventoryItemCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateInventoryItemCommand command, CancellationToken ct)
    {
        var entity = InventoryItem.Create(
            command.Name,
            currentUser.GetTenant() ?? "root",
            currentUser.GetUserId(),
            currentUser.Name ?? "System",
            command.Description);
        
        context.InventoryItems.Add(entity);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
