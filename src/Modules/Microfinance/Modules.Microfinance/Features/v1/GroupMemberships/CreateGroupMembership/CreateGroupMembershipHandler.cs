using FSH.Framework.Core.Context;
using FSH.Framework.Core.Exceptions;
using FSH.Modules.Microfinance.Data;
using FSH.Modules.Microfinance.Domain;

namespace FSH.Modules.Microfinance.Features.v1.GroupMemberships.CreateGroupMembership;

public record CreateGroupMembershipCommand(string Name) : ICommand<Guid>;

public class CreateGroupMembershipHandler(ICurrentUser currentUser,
    MicrofinanceDbContext context) : ICommandHandler<CreateGroupMembershipCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateGroupMembershipCommand command, CancellationToken ct)
    {
        var entity = GroupMembership.Create(
            command.Name,
            currentUser.GetTenant() ?? "root",
            currentUser.GetUserId(),
            currentUser.Name ?? "System");
        
        context.GroupMemberships.Add(entity);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
