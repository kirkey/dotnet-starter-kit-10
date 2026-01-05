using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.CollectionCases.DeleteCollectionCase;

namespace FSH.Module.Microfinance.Features.v1.CollectionCases.DeleteCollectionCase;

public class DeleteCollectionCaseHandler(MicrofinanceDbContext context) : ICommandHandler<DeleteCollectionCaseCommand>
{
    public async ValueTask<Unit> Handle(DeleteCollectionCaseCommand command, CancellationToken ct)
    {
        var entity = await context.CollectionCases.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("CollectionCase not found");
        
        context.CollectionCases.Remove(entity);
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
