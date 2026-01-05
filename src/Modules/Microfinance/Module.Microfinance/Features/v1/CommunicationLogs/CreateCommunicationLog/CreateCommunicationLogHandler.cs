using FSH.Framework.Core.Context;
using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;
using FSH.Module.Microfinance.Domain;

using FSH.Module.Microfinance.Contracts.v1.CommunicationLogs.CreateCommunicationLog;

namespace FSH.Module.Microfinance.Features.v1.CommunicationLogs.CreateCommunicationLog;

public class CreateCommunicationLogHandler(ICurrentUser currentUser,
    MicrofinanceDbContext context) : ICommandHandler<CreateCommunicationLogCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateCommunicationLogCommand command, CancellationToken ct)
    {
        var entity = CommunicationLog.Create(
            command.Name,
            currentUser.GetTenant() ?? "root",
            currentUser.GetUserId(),
            currentUser.Name ?? "System");
        
        context.CommunicationLogs.Add(entity);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
