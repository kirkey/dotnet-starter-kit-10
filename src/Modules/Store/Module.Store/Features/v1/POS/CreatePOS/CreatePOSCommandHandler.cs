using FSH.Framework.Core.Context;
using FSH.Module.Store.Contracts.v1.POS;
using FSH.Module.Store.Data;
using FSH.Module.Store.Domain;
using Mediator;

namespace FSH.Module.Store.Features.v1.POS.CreatePOS;

public sealed class CreatePOSCommandHandler(
    StoreDbContext context,
    ICurrentUser currentUser)
    : ICommandHandler<CreatePOSCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreatePOSCommand command, CancellationToken cancellationToken)
    {
        var pos = POSEntity.Create(
            command.Name,
            command.Identifier,
            command.StoreId,
            currentUser.GetTenant() ?? "root",
            currentUser.GetUserId(),
            currentUser.Name ?? "System",
            command.Description,
            command.Location);
        
        context.POSTerminals.Add(pos);
        await context.SaveChangesAsync(cancellationToken);
        
        return pos.Id;
    }
}
