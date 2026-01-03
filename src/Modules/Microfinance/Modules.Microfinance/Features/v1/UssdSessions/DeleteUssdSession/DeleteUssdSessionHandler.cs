using FSH.Framework.Core.Exceptions;
using FSH.Modules.Microfinance.Data;

namespace FSH.Modules.Microfinance.Features.v1.UssdSessions.DeleteUssdSession;

public record DeleteUssdSessionCommand(Guid Id) : ICommand;

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
