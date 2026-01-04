using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

namespace FSH.Module.Microfinance.Features.v1.TellerSessions.UpdateTellerSession;

public record UpdateTellerSessionCommand(Guid Id, string Name) : ICommand<Guid>;

public class UpdateTellerSessionHandler(MicrofinanceDbContext context) : ICommandHandler<UpdateTellerSessionCommand, Guid>
{
    public async ValueTask<Guid> Handle(UpdateTellerSessionCommand command, CancellationToken ct)
    {
        var entity = await context.TellerSessions.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("TellerSession not found");
        
        entity.Update(command.Name);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
