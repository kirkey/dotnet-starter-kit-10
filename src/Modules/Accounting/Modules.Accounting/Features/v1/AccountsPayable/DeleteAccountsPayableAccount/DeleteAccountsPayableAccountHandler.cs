using FSH.Framework.Core.Exceptions;
using FSH.Modules.Accounting.Data;
using Mediator;

namespace FSH.Modules.Accounting.Features.v1.AccountsPayable.DeleteAccountsPayableAccount;

public record DeleteAccountsPayableAccountCommand(Guid Id) : ICommand;

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
