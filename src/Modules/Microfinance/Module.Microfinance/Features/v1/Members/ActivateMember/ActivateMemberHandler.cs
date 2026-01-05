using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.Members;namespace FSH.Module.Microfinance.Features.v1.Members.ActivateMember;
public class ActivateMemberHandler(MicrofinanceDbContext context) : ICommandHandler<ActivateMemberCommand>
{
    public async ValueTask<Unit> Handle(ActivateMemberCommand command, CancellationToken ct)
    {
        var member = await context.Members.FindAsync(command.Id, ct)
            ?? throw new NotFoundException("Member not found");
        member.Activate();
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
