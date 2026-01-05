using FSH.Framework.Core.Context;
using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;
using FSH.Module.Microfinance.Domain;

using FSH.Module.Microfinance.Contracts.v1.MemberGroups.CreateMemberGroup;

namespace FSH.Module.Microfinance.Features.v1.MemberGroups.CreateMemberGroup;

public class CreateMemberGroupHandler(ICurrentUser currentUser,
    MicrofinanceDbContext context) : ICommandHandler<CreateMemberGroupCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateMemberGroupCommand command, CancellationToken ct)
    {
        var entity = MemberGroup.Create(
            command.Name,
            currentUser.GetTenant() ?? "root",
            currentUser.GetUserId(),
            currentUser.Name ?? "System");
        
        context.MemberGroups.Add(entity);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
