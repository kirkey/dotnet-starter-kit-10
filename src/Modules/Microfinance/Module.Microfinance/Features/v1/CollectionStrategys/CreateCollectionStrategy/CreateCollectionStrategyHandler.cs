using FSH.Framework.Core.Context;
using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;
using FSH.Module.Microfinance.Domain;

using FSH.Module.Microfinance.Contracts.v1.CollectionStrategys.CreateCollectionStrategy;

namespace FSH.Module.Microfinance.Features.v1.CollectionStrategys.CreateCollectionStrategy;

public class CreateCollectionStrategyHandler(ICurrentUser currentUser,
    MicrofinanceDbContext context) : ICommandHandler<CreateCollectionStrategyCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateCollectionStrategyCommand command, CancellationToken ct)
    {
        var entity = CollectionStrategy.Create(
            command.Name,
            currentUser.GetTenant() ?? "root",
            currentUser.GetUserId(),
            currentUser.Name ?? "System");
        
        context.CollectionStrategys.Add(entity);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
