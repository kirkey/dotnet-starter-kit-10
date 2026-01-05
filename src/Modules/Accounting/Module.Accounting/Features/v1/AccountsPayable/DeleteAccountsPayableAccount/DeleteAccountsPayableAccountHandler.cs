using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Data;
using Mediator;
using FSH.Module.Accounting.Contracts.v1.AccountsPayable.DeleteAccountsPayableAccount;

namespace FSH.Module.Accounting.Features.v1.AccountsPayable.DeleteAccountsPayableAccount;

public class DeleteAccountsPayableAccountHandler(AccountingDbContext context) : ICommandHandler<DeleteAccountsPayableAccountCommand>
{
    public async ValueTask<Unit> Handle(DeleteAccountsPayableAccountCommand command, CancellationToken ct)
    {
        var entity = await context.AccountsPayable.FindAsync(command.Id, ct)
            ?? throw new NotFoundException("AccountsPayableAccount not found");
        
        context.AccountsPayable.Remove(entity);
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
