using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.CollectionActions.UpdateCollectionAction;

namespace FSH.Module.Microfinance.Features.v1.CollectionActions.UpdateCollectionAction;

public class UpdateCollectionActionHandler(MicrofinanceDbContext context) : ICommandHandler<UpdateCollectionActionCommand, Guid>
{
    public async ValueTask<Guid> Handle(UpdateCollectionActionCommand command, CancellationToken ct)
    {
        var entity = await context.CollectionActions.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("CollectionAction not found");
        
        entity.Update(command.Name);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
