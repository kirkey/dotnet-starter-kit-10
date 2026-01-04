using FSH.Framework.Core.Exceptions;
using FSH.Modules.Accounting.Data;
using Mediator;

namespace FSH.Modules.Accounting.Features.v1.Members.DeleteMember;

public record DeleteMemberCommand(Guid Id) : ICommand;

public class DeleteMemberHandler(AccountingDbContext context) : ICommandHandler<DeleteMemberCommand>
{
    public async ValueTask<Unit> Handle(DeleteMemberCommand command, CancellationToken ct)
    {
        var entity = await context.Members.FindAsync(command.Id, ct)
            ?? throw new NotFoundException("Member not found");
        
        context.Members.Remove(entity);
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
