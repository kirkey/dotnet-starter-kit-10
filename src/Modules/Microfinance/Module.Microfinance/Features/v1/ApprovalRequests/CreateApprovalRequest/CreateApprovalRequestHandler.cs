using FSH.Framework.Core.Context;
using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;
using FSH.Module.Microfinance.Domain;

namespace FSH.Module.Microfinance.Features.v1.ApprovalRequests.CreateApprovalRequest;

public record CreateApprovalRequestCommand(string Name) : ICommand<Guid>;

public class CreateApprovalRequestHandler(ICurrentUser currentUser,
    MicrofinanceDbContext context) : ICommandHandler<CreateApprovalRequestCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateApprovalRequestCommand command, CancellationToken ct)
    {
        var entity = ApprovalRequest.Create(
            command.Name,
            currentUser.GetTenant() ?? "root",
            currentUser.GetUserId(),
            currentUser.Name ?? "System");
        
        context.ApprovalRequests.Add(entity);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
