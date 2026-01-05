using FSH.Framework.Shared.Identity;
using FSH.Module.Accounting.Data;
using FSH.Module.Accounting.Domain;
using Mediator;
using FSH.Module.Accounting.Contracts.v1.AccountsReceivable.CreateAccountsReceivableAccount;

namespace FSH.Module.Accounting.Features.v1.AccountsReceivable.CreateAccountsReceivableAccount;

public class CreateAccountsReceivableAccountHandler(AccountingDbContext context, ICurrentUser currentUser) 
    : ICommandHandler<CreateAccountsReceivableAccountCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateAccountsReceivableAccountCommand command, CancellationToken ct)
    {
        var entity = AccountsReceivableAccount.Create(
            command.Name,
            currentUser.GetTenant() ?? "root",
            currentUser.GetUserId(),
            currentUser.Name ?? "System",
            command.Description);
        
        context.AccountsReceivable.Add(entity);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
