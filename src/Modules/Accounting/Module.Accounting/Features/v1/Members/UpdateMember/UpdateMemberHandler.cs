using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Data;
using Mediator;

using FSH.Module.Accounting.Contracts.v1.Members.UpdateMember;namespace FSH.Module.Accounting.Features.v1.Members.UpdateMember;

public class UpdateMemberHandler(AccountingDbContext context) : ICommandHandler<UpdateMemberCommand, Guid>
{
    public async ValueTask<Guid> Handle(UpdateMemberCommand command, CancellationToken ct)
    {
        var entity = await context.Members.FindAsync(command.Id, ct)
            ?? throw new NotFoundException("Member not found");
        
        entity.Update(command.Name, command.Description);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
