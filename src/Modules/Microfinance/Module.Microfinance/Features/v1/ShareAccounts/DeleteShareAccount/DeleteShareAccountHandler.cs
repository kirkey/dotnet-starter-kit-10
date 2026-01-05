using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.ShareAccounts.DeleteShareAccount;

namespace FSH.Module.Microfinance.Features.v1.ShareAccounts.DeleteShareAccount;

public class DeleteShareAccountHandler(MicrofinanceDbContext context) : ICommandHandler<DeleteShareAccountCommand>
{
    public async ValueTask<Unit> Handle(DeleteShareAccountCommand command, CancellationToken ct)
    {
        var entity = await context.ShareAccounts.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("ShareAccount not found");
        
        context.ShareAccounts.Remove(entity);
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
