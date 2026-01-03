using FSH.Framework.Core.Context;
using FSH.Framework.Core.Exceptions;
using FSH.Modules.Microfinance.Data;
using FSH.Modules.Microfinance.Domain;

namespace FSH.Modules.Microfinance.Features.v1.ShareAccounts.CreateShareAccount;

public record CreateShareAccountCommand(string Name) : ICommand<Guid>;

public class CreateShareAccountHandler(ICurrentUser currentUser,
    MicrofinanceDbContext context) : ICommandHandler<CreateShareAccountCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateShareAccountCommand command, CancellationToken ct)
    {
        var entity = ShareAccount.Create(
            command.Name,
            currentUser.GetTenant() ?? "root",
            currentUser.GetUserId(),
            currentUser.Name ?? "System");
        
        context.ShareAccounts.Add(entity);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
