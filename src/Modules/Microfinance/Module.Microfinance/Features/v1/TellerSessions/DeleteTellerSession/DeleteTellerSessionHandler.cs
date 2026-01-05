using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.TellerSessions.DeleteTellerSession;

namespace FSH.Module.Microfinance.Features.v1.TellerSessions.DeleteTellerSession;

public class DeleteTellerSessionHandler(MicrofinanceDbContext context) : ICommandHandler<DeleteTellerSessionCommand>
{
    public async ValueTask<Unit> Handle(DeleteTellerSessionCommand command, CancellationToken ct)
    {
        var entity = await context.TellerSessions.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("TellerSession not found");
        
        context.TellerSessions.Remove(entity);
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
