using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.GroupMemberships.DeleteGroupMembership;

namespace FSH.Module.Microfinance.Features.v1.GroupMemberships.DeleteGroupMembership;

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
