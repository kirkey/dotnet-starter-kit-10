using FSH.Framework.Core.Context;
using FSH.Module.Store.Contracts.v1.Stores;
using FSH.Module.Store.Data;
using Mediator;

namespace FSH.Module.Store.Features.v1.Stores.CreateStore;

public sealed class CreateStoreCommandHandler(
    StoreDbContext context,
    ICurrentUser currentUser)
    : ICommandHandler<CreateStoreCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateStoreCommand command, CancellationToken cancellationToken)
    {
        var store = Domain.Store.Create(
            command.Name,
            command.Address,
            command.City,
            command.PostalCode,
            currentUser.GetTenant() ?? "root",
            currentUser.GetUserId(),
            currentUser.Name ?? "System",
            command.Description,
            command.State,
            command.Phone,
            command.Email);
        
        context.Stores.Add(store);
        await context.SaveChangesAsync(cancellationToken);
        
        return store.Id;
    }
}
