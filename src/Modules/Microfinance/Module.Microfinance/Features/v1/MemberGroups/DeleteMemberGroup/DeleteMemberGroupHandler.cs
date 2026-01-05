using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.MemberGroups.DeleteMemberGroup;

namespace FSH.Module.Microfinance.Features.v1.MemberGroups.DeleteMemberGroup;

public class DeleteMemberGroupHandler(MicrofinanceDbContext context) : ICommandHandler<DeleteMemberGroupCommand>
{
    public async ValueTask<Unit> Handle(DeleteMemberGroupCommand command, CancellationToken ct)
    {
        var entity = await context.MemberGroups.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("MemberGroup not found");
        
        context.MemberGroups.Remove(entity);
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
