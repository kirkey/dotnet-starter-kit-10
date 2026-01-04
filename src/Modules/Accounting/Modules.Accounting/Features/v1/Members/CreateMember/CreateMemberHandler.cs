using FSH.Framework.Core.Identity;
using FSH.Modules.Accounting.Data;
using FSH.Modules.Accounting.Domain;
using Mediator;

namespace FSH.Modules.Accounting.Features.v1.Members.CreateMember;

public record CreateMemberCommand(string Name, string? Description) : ICommand<Guid>;

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
