using FSH.Framework.Core.Context;
using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;
using FSH.Module.Microfinance.Domain;

using FSH.Module.Microfinance.Contracts.v1.GroupMemberships.CreateGroupMembership;

namespace FSH.Module.Microfinance.Features.v1.GroupMemberships.CreateGroupMembership;

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
