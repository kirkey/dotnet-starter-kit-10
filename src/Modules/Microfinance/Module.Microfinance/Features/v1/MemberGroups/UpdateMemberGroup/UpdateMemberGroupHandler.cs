using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.MemberGroups.UpdateMemberGroup;

namespace FSH.Module.Microfinance.Features.v1.MemberGroups.UpdateMemberGroup;

public class UpdateMemberGroupHandler(MicrofinanceDbContext context) : ICommandHandler<UpdateMemberGroupCommand, Guid>
{
    public async ValueTask<Guid> Handle(UpdateMemberGroupCommand command, CancellationToken ct)
    {
        var entity = await context.MemberGroups.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("MemberGroup not found");
        
        entity.Update(command.Name);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
