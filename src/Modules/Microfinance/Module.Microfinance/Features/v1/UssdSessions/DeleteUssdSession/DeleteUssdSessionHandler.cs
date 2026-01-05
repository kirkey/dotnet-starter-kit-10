using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.UssdSessions.DeleteUssdSession;

namespace FSH.Module.Microfinance.Features.v1.UssdSessions.DeleteUssdSession;

public class DeleteUssdSessionHandler(MicrofinanceDbContext context) : ICommandHandler<DeleteUssdSessionCommand>
{
    public async ValueTask<Unit> Handle(DeleteUssdSessionCommand command, CancellationToken ct)
    {
        var entity = await context.UssdSessions.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("UssdSession not found");
        
        context.UssdSessions.Remove(entity);
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
