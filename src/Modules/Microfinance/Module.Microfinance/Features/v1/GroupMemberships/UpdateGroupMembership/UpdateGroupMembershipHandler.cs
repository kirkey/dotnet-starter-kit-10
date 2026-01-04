using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

namespace FSH.Module.Microfinance.Features.v1.GroupMemberships.UpdateGroupMembership;

public record UpdateGroupMembershipCommand(Guid Id, string Name) : ICommand<Guid>;

public class UpdateGroupMembershipHandler(MicrofinanceDbContext context) : ICommandHandler<UpdateGroupMembershipCommand, Guid>
{
    public async ValueTask<Guid> Handle(UpdateGroupMembershipCommand command, CancellationToken ct)
    {
        var entity = await context.GroupMemberships.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("GroupMembership not found");
        
        entity.Update(command.Name);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
