using FSH.Framework.Core.Context;
using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;
using FSH.Module.Microfinance.Domain;

namespace FSH.Module.Microfinance.Features.v1.UssdSessions.CreateUssdSession;

public record CreateUssdSessionCommand(string Name) : ICommand<Guid>;

public class CreateUssdSessionHandler(ICurrentUser currentUser,
    MicrofinanceDbContext context) : ICommandHandler<CreateUssdSessionCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateUssdSessionCommand command, CancellationToken ct)
    {
        var entity = UssdSession.Create(
            command.Name,
            currentUser.GetTenant() ?? "root",
            currentUser.GetUserId(),
            currentUser.Name ?? "System");
        
        context.UssdSessions.Add(entity);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
