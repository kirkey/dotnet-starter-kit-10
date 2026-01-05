using FSH.Framework.Core.Context;
using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;
using FSH.Module.Microfinance.Domain;

using FSH.Module.Microfinance.Contracts.v1.CollectionCases.CreateCollectionCase;

namespace FSH.Module.Microfinance.Features.v1.CollectionCases.CreateCollectionCase;

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
