using FSH.Framework.Core.Exceptions;
using FSH.Modules.Microfinance.Data;

namespace FSH.Modules.Microfinance.Features.v1.CollectionStrategys.UpdateCollectionStrategy;

public record UpdateCollectionStrategyCommand(Guid Id, string Name) : ICommand<Guid>;

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
