using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

namespace FSH.Module.Microfinance.Features.v1.CollectionCases.UpdateCollectionCase;

public record UpdateCollectionCaseCommand(Guid Id, string Name) : ICommand<Guid>;

public class UpdateCollectionCaseHandler(MicrofinanceDbContext context) : ICommandHandler<UpdateCollectionCaseCommand, Guid>
{
    public async ValueTask<Guid> Handle(UpdateCollectionCaseCommand command, CancellationToken ct)
    {
        var entity = await context.CollectionCases.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("CollectionCase not found");
        
        entity.Update(command.Name);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
