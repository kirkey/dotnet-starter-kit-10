using FSH.Framework.Core.Context;
using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;
using FSH.Module.Microfinance.Domain;

using FSH.Module.Microfinance.Contracts.v1.ShareAccounts.CreateShareAccount;

namespace FSH.Module.Microfinance.Features.v1.ShareAccounts.CreateShareAccount;

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
