using FSH.Framework.Shared.Identity;
using FSH.Module.Accounting.Data;
using FSH.Module.Accounting.Domain;
using Mediator;

using FSH.Module.Accounting.Contracts.v1.Members.CreateMember;namespace FSH.Module.Accounting.Features.v1.Members.CreateMember;

public class CreateMemberHandler(AccountingDbContext context, ICurrentUser currentUser) 
    : ICommandHandler<CreateMemberCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateMemberCommand command, CancellationToken ct)
    {
        var entity = Member.Create(
            command.Name,
            currentUser.GetTenant() ?? "root",
            currentUser.GetUserId(),
            currentUser.Name ?? "System",
            command.Description);
        
        context.Members.Add(entity);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
