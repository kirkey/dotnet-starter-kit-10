using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.CollectionActions.DeleteCollectionAction;

namespace FSH.Module.Microfinance.Features.v1.CollectionActions.DeleteCollectionAction;

public class DeleteCollectionActionHandler(MicrofinanceDbContext context) : ICommandHandler<DeleteCollectionActionCommand>
{
    public async ValueTask<Unit> Handle(DeleteCollectionActionCommand command, CancellationToken ct)
    {
        var entity = await context.CollectionActions.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("CollectionAction not found");
        
        context.CollectionActions.Remove(entity);
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
