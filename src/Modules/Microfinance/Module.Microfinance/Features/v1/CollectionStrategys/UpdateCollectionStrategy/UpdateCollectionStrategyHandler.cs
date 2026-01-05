using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.CollectionStrategys.UpdateCollectionStrategy;

namespace FSH.Module.Microfinance.Features.v1.CollectionStrategys.UpdateCollectionStrategy;

public class UpdateCollectionStrategyHandler(MicrofinanceDbContext context) : ICommandHandler<UpdateCollectionStrategyCommand, Guid>
{
    public async ValueTask<Guid> Handle(UpdateCollectionStrategyCommand command, CancellationToken ct)
    {
        var entity = await context.CollectionStrategys.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("CollectionStrategy not found");
        
        entity.Update(command.Name);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
