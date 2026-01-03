using FSH.Framework.Core.Context;
using FSH.Framework.Core.Exceptions;
using FSH.Modules.Microfinance.Data;
using FSH.Modules.Microfinance.Domain;

namespace FSH.Modules.Microfinance.Features.v1.CommunicationLogs.CreateCommunicationLog;

public record CreateCommunicationLogCommand(string Name) : ICommand<Guid>;

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
