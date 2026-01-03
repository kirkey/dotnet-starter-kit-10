using FSH.Framework.Core.Exceptions;
using FSH.Modules.Microfinance.Data;

namespace FSH.Modules.Microfinance.Features.v1.ApprovalRequests.DeleteApprovalRequest;

public record DeleteApprovalRequestCommand(Guid Id) : ICommand;

public class DeleteApprovalRequestHandler(MicrofinanceDbContext context) : ICommandHandler<DeleteApprovalRequestCommand>
{
    public async ValueTask<Unit> Handle(DeleteApprovalRequestCommand command, CancellationToken ct)
    {
        var entity = await context.ApprovalRequests.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("ApprovalRequest not found");
        
        context.ApprovalRequests.Remove(entity);
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
