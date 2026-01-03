using FSH.Framework.Core.Context;
using FSH.Framework.Core.Exceptions;
using FSH.Modules.Microfinance.Data;
using FSH.Modules.Microfinance.Domain;

namespace FSH.Modules.Microfinance.Features.v1.CollectionCases.CreateCollectionCase;

public record CreateCollectionCaseCommand(string Name) : ICommand<Guid>;

public class CreateCollectionCaseHandler(ICurrentUser currentUser,
    MicrofinanceDbContext context) : ICommandHandler<CreateCollectionCaseCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateCollectionCaseCommand command, CancellationToken ct)
    {
        var entity = CollectionCase.Create(
            command.Name,
            currentUser.GetTenant() ?? "root",
            currentUser.GetUserId(),
            currentUser.Name ?? "System");
        
        context.CollectionCases.Add(entity);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
