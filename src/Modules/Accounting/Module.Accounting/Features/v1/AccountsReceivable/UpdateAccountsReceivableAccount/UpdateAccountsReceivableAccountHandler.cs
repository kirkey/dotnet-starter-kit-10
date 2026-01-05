using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Data;
using Mediator;
using FSH.Module.Accounting.Contracts.v1.AccountsReceivable.UpdateAccountsReceivableAccount;

namespace FSH.Module.Accounting.Features.v1.AccountsReceivable.UpdateAccountsReceivableAccount;

public class UpdateAccountsReceivableAccountHandler(AccountingDbContext context) : ICommandHandler<UpdateAccountsReceivableAccountCommand, Guid>
{
    public async ValueTask<Guid> Handle(UpdateAccountsReceivableAccountCommand command, CancellationToken ct)
    {
        var entity = await context.AccountsReceivable.FindAsync(command.Id, ct)
            ?? throw new NotFoundException("AccountsReceivableAccount not found");
        
        entity.Update(command.Name, command.Description);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
