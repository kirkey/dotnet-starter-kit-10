using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

namespace FSH.Module.Microfinance.Features.v1.GroupMemberships.DeleteGroupMembership;

public record DeleteGroupMembershipCommand(Guid Id) : ICommand;

public class DeleteGroupMembershipHandler(MicrofinanceDbContext context) : ICommandHandler<DeleteGroupMembershipCommand>
{
    public async ValueTask<Unit> Handle(DeleteGroupMembershipCommand command, CancellationToken ct)
    {
        var entity = await context.GroupMemberships.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("GroupMembership not found");
        
        context.GroupMemberships.Remove(entity);
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
