using FSH.Framework.Core.Identity;
using FSH.Modules.Accounting.Data;
using FSH.Modules.Accounting.Domain;
using Mediator;

namespace FSH.Modules.Accounting.Features.v1.AccountsReceivable.CreateAccountsReceivableAccount;

public record CreateAccountsReceivableAccountCommand(string Name, string? Description) : ICommand<Guid>;

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
