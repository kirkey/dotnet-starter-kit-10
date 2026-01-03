using FSH.Framework.Core.Exceptions;
using FSH.Modules.Microfinance.Data;

namespace FSH.Modules.Microfinance.Features.v1.CollectionCases.DeleteCollectionCase;

public record DeleteCollectionCaseCommand(Guid Id) : ICommand;

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
