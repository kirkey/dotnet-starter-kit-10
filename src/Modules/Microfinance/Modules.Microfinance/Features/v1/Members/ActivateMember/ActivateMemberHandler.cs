using FSH.Modules.Microfinance.Data;
using FSH.Framework.Core.Exceptions;

namespace FSH.Modules.Microfinance.Features.v1.Members.ActivateMember;
public record ActivateMemberCommand(Guid MemberId) : ICommand;
public class ActivateMemberHandler(MicrofinanceDbContext context) : ICommandHandler<ActivateMemberCommand>
{
    public async ValueTask<Unit> Handle(ActivateMemberCommand command, CancellationToken ct)
    {
        var member = await context.Members.FindAsync([command.MemberId], ct)
            ?? throw new NotFoundException("Member not found");
        member.Activate();
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
