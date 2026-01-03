using FSH.Framework.Core.Exceptions;
using FSH.Modules.Microfinance.Data;

namespace FSH.Modules.Microfinance.Features.v1.ShareAccounts.DeleteShareAccount;

public record DeleteShareAccountCommand(Guid Id) : ICommand;

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
