using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.UssdSessions.UpdateUssdSession;

namespace FSH.Module.Microfinance.Features.v1.UssdSessions.UpdateUssdSession;

public class UpdateUssdSessionHandler(MicrofinanceDbContext context) : ICommandHandler<UpdateUssdSessionCommand, Guid>
{
    public async ValueTask<Guid> Handle(UpdateUssdSessionCommand command, CancellationToken ct)
    {
        var entity = await context.UssdSessions.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("UssdSession not found");
        
        entity.Update(command.Name);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
