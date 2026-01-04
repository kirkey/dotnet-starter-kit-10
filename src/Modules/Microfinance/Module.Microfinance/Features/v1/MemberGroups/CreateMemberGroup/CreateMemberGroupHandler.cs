using FSH.Framework.Core.Context;
using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;
using FSH.Module.Microfinance.Domain;

namespace FSH.Module.Microfinance.Features.v1.MemberGroups.CreateMemberGroup;

public record CreateMemberGroupCommand(string Name) : ICommand<Guid>;

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
