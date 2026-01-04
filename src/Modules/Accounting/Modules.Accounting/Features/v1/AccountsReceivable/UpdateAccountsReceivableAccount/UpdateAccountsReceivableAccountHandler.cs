using FSH.Framework.Core.Exceptions;
using FSH.Modules.Accounting.Data;
using Mediator;

namespace FSH.Modules.Accounting.Features.v1.AccountsReceivable.UpdateAccountsReceivableAccount;

public record UpdateAccountsReceivableAccountCommand(Guid Id, string Name, string? Description) : ICommand<Guid>;

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
