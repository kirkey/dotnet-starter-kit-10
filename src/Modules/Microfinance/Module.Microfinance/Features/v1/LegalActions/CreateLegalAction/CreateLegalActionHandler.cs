using FSH.Framework.Core.Context;
using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;
using FSH.Module.Microfinance.Domain;

using FSH.Module.Microfinance.Contracts.v1.LegalActions.CreateLegalAction;

namespace FSH.Module.Microfinance.Features.v1.LegalActions.CreateLegalAction;

public class CreateLegalActionHandler(ICurrentUser currentUser,
    MicrofinanceDbContext context) : ICommandHandler<CreateLegalActionCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateLegalActionCommand command, CancellationToken ct)
    {
        var entity = LegalAction.Create(
            command.Name,
            currentUser.GetTenant() ?? "root",
            currentUser.GetUserId(),
            currentUser.Name ?? "System");
        
        context.LegalActions.Add(entity);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
