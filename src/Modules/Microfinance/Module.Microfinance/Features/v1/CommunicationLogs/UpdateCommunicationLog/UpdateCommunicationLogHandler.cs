using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.CommunicationLogs.UpdateCommunicationLog;

namespace FSH.Module.Microfinance.Features.v1.CommunicationLogs.UpdateCommunicationLog;

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
