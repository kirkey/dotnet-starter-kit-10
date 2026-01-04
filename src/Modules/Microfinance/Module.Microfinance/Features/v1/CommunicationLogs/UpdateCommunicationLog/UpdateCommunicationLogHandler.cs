using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

namespace FSH.Module.Microfinance.Features.v1.CommunicationLogs.UpdateCommunicationLog;

public record UpdateCommunicationLogCommand(Guid Id, string Name) : ICommand<Guid>;

public class UpdateCommunicationLogHandler(MicrofinanceDbContext context) : ICommandHandler<UpdateCommunicationLogCommand, Guid>
{
    public async ValueTask<Guid> Handle(UpdateCommunicationLogCommand command, CancellationToken ct)
    {
        var entity = await context.CommunicationLogs.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("CommunicationLog not found");
        
        entity.Update(command.Name);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
