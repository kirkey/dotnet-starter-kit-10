using FSH.Framework.Core.Context;
using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;
using FSH.Module.Microfinance.Domain;

using FSH.Module.Microfinance.Contracts.v1.TellerSessions.CreateTellerSession;

namespace FSH.Module.Microfinance.Features.v1.TellerSessions.CreateTellerSession;

public class CreateTellerSessionHandler(ICurrentUser currentUser,
    MicrofinanceDbContext context) : ICommandHandler<CreateTellerSessionCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateTellerSessionCommand command, CancellationToken ct)
    {
        var entity = TellerSession.Create(
            command.Name,
            currentUser.GetTenant() ?? "root",
            currentUser.GetUserId(),
            currentUser.Name ?? "System");
        
        context.TellerSessions.Add(entity);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
