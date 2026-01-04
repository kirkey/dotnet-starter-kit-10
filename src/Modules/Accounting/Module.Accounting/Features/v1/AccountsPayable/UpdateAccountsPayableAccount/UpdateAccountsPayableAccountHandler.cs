using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Data;
using Mediator;

namespace FSH.Module.Accounting.Features.v1.AccountsPayable.UpdateAccountsPayableAccount;

public record UpdateAccountsPayableAccountCommand(Guid Id, string Name, string? Description) : ICommand<Guid>;

public class UpdateAccountsPayableAccountHandler(AccountingDbContext context) : ICommandHandler<UpdateAccountsPayableAccountCommand, Guid>
{
    public async ValueTask<Guid> Handle(UpdateAccountsPayableAccountCommand command, CancellationToken ct)
    {
        var entity = await context.AccountsPayable.FindAsync(command.Id, ct)
            ?? throw new NotFoundException("AccountsPayableAccount not found");
        
        entity.Update(command.Name, command.Description);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
