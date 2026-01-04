using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

namespace FSH.Module.Microfinance.Features.v1.CollectionStrategys.DeleteCollectionStrategy;

public record DeleteCollectionStrategyCommand(Guid Id) : ICommand;

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
