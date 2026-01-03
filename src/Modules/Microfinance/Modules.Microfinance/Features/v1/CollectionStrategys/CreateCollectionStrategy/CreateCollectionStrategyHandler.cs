using FSH.Framework.Core.Context;
using FSH.Framework.Core.Exceptions;
using FSH.Modules.Microfinance.Data;
using FSH.Modules.Microfinance.Domain;

namespace FSH.Modules.Microfinance.Features.v1.CollectionStrategys.CreateCollectionStrategy;

public record CreateCollectionStrategyCommand(string Name) : ICommand<Guid>;

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
