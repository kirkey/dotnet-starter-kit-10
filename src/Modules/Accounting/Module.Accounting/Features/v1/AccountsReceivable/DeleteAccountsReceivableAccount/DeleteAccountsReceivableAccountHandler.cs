using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Data;
using Mediator;

namespace FSH.Module.Accounting.Features.v1.AccountsReceivable.DeleteAccountsReceivableAccount;

public record DeleteAccountsReceivableAccountCommand(Guid Id) : ICommand;

public class DeleteAccountsReceivableAccountHandler(AccountingDbContext context) : ICommandHandler<DeleteAccountsReceivableAccountCommand>
{
    public async ValueTask<Unit> Handle(DeleteAccountsReceivableAccountCommand command, CancellationToken ct)
    {
        var entity = await context.AccountsReceivable.FindAsync(command.Id, ct)
            ?? throw new NotFoundException("AccountsReceivableAccount not found");
        
        context.AccountsReceivable.Remove(entity);
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
