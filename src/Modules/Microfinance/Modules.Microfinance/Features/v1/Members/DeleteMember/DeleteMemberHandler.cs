using FSH.Modules.Microfinance.Data;
using FSH.Framework.Core.Exceptions;

namespace FSH.Modules.Microfinance.Features.v1.Members.DeleteMember;
public record DeleteMemberCommand(Guid MemberId) : ICommand;
public class DeleteMemberHandler(MicrofinanceDbContext context) : ICommandHandler<DeleteMemberCommand>
{
    public async ValueTask<Unit> Handle(DeleteMemberCommand command, CancellationToken ct)
    {
        var member = await context.Members.FindAsync([command.MemberId], ct)
            ?? throw new NotFoundException("Member not found");
        context.Members.Remove(member);
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
