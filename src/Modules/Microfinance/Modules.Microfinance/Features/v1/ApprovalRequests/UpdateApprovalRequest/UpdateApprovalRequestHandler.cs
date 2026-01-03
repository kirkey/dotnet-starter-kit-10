using FSH.Framework.Core.Exceptions;
using FSH.Modules.Microfinance.Data;

namespace FSH.Modules.Microfinance.Features.v1.ApprovalRequests.UpdateApprovalRequest;

public record UpdateApprovalRequestCommand(Guid Id, string Name) : ICommand<Guid>;

public class UpdateApprovalRequestHandler(MicrofinanceDbContext context) : ICommandHandler<UpdateApprovalRequestCommand, Guid>
{
    public async ValueTask<Guid> Handle(UpdateApprovalRequestCommand command, CancellationToken ct)
    {
        var entity = await context.ApprovalRequests.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("ApprovalRequest not found");
        
        entity.Update(command.Name);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
