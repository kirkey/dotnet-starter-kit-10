using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

namespace FSH.Module.Microfinance.Features.v1.MobileTransactions.DeleteMobileTransaction;

public record DeleteMobileTransactionCommand(Guid Id) : ICommand;

public class DeleteMobileTransactionHandler(MicrofinanceDbContext context) : ICommandHandler<DeleteMobileTransactionCommand>
{
    public async ValueTask<Unit> Handle(DeleteMobileTransactionCommand command, CancellationToken ct)
    {
        var entity = await context.MobileTransactions.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("MobileTransaction not found");
        
        context.MobileTransactions.Remove(entity);
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
