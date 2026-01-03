using FSH.Framework.Core.Exceptions;
using FSH.Modules.Microfinance.Data;

namespace FSH.Modules.Microfinance.Features.v1.Members.DeactivateMember;
public record DeactivateMemberCommand(Guid MemberId) : ICommand;
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
