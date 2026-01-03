using FSH.Framework.Core.Context;
using FSH.Framework.Core.Exceptions;
using FSH.Modules.Microfinance.Data;
using FSH.Modules.Microfinance.Domain;

namespace FSH.Modules.Microfinance.Features.v1.TellerSessions.CreateTellerSession;

public record CreateTellerSessionCommand(string Name) : ICommand<Guid>;

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
