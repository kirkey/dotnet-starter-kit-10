using FSH.Framework.Shared.Identity;
using FSH.Module.Accounting.Data;
using FSH.Module.Accounting.Domain;
using Mediator;

namespace FSH.Module.Accounting.Features.v1.AccountsPayable.CreateAccountsPayableAccount;

public record CreateAccountsPayableAccountCommand(string Name, string? Description) : ICommand<Guid>;

public class CreateAccountsPayableAccountHandler(AccountingDbContext context, ICurrentUser currentUser) 
    : ICommandHandler<CreateAccountsPayableAccountCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateAccountsPayableAccountCommand command, CancellationToken ct)
    {
        var entity = AccountsPayableAccount.Create(
            command.Name,
            currentUser.GetTenant() ?? "root",
            currentUser.GetUserId(),
            currentUser.Name ?? "System",
            command.Description);
        
        context.AccountsPayable.Add(entity);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
