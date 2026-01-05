using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.CollectionStrategys.DeleteCollectionStrategy;

namespace FSH.Module.Microfinance.Features.v1.CollectionStrategys.DeleteCollectionStrategy;

public class DeleteCollectionStrategyHandler(MicrofinanceDbContext context) : ICommandHandler<DeleteCollectionStrategyCommand>
{
    public async ValueTask<Unit> Handle(DeleteCollectionStrategyCommand command, CancellationToken ct)
    {
        var entity = await context.CollectionStrategys.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("CollectionStrategy not found");
        
        context.CollectionStrategys.Remove(entity);
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
