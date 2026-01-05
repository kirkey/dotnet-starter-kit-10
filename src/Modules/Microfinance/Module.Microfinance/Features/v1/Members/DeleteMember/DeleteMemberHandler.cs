using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.Members;namespace FSH.Module.Microfinance.Features.v1.Members.DeleteMember;
public class DeleteMemberHandler(MicrofinanceDbContext context) : ICommandHandler<DeleteMemberCommand>
{
    public async ValueTask<Unit> Handle(DeleteMemberCommand command, CancellationToken ct)
    {
        var member = await context.Members.FindAsync(command.Id, ct)
            ?? throw new NotFoundException("Member not found");
        context.Members.Remove(member);
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
