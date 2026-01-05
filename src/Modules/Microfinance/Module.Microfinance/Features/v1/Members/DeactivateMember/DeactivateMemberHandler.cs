using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.Members;namespace FSH.Module.Microfinance.Features.v1.Members.DeactivateMember;
public class DeactivateMemberHandler(MicrofinanceDbContext context) : ICommandHandler<DeactivateMemberCommand>
{
    public async ValueTask<Unit> Handle(DeactivateMemberCommand command, CancellationToken ct)
    {
        var member = await context.Members.FindAsync([command.MemberId], ct)
            ?? throw new NotFoundException("Member not found");
        member.Deactivate();
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
