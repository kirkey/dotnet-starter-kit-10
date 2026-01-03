using FSH.Framework.Core.Context;
using FSH.Framework.Core.Exceptions;
using FSH.Modules.Microfinance.Data;
using FSH.Modules.Microfinance.Domain;

namespace FSH.Modules.Microfinance.Features.v1.CollectionActions.CreateCollectionAction;

public record CreateCollectionActionCommand(string Name) : ICommand<Guid>;

public class CreateCollectionActionHandler(ICurrentUser currentUser,
    MicrofinanceDbContext context) : ICommandHandler<CreateCollectionActionCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateCollectionActionCommand command, CancellationToken ct)
    {
        var entity = CollectionAction.Create(
            command.Name,
            currentUser.GetTenant() ?? "root",
            currentUser.GetUserId(),
            currentUser.Name ?? "System");
        
        context.CollectionActions.Add(entity);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
