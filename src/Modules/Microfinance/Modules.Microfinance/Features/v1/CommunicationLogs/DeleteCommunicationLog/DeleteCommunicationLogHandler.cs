using FSH.Framework.Core.Exceptions;
using FSH.Modules.Microfinance.Data;

namespace FSH.Modules.Microfinance.Features.v1.CommunicationLogs.DeleteCommunicationLog;

public record DeleteCommunicationLogCommand(Guid Id) : ICommand;

public class DeleteCommunicationLogHandler(MicrofinanceDbContext context) : ICommandHandler<DeleteCommunicationLogCommand>
{
    public async ValueTask<Unit> Handle(DeleteCommunicationLogCommand command, CancellationToken ct)
    {
        var entity = await context.CommunicationLogs.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("CommunicationLog not found");
        
        context.CommunicationLogs.Remove(entity);
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
