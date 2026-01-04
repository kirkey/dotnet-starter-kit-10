using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

namespace FSH.Module.Microfinance.Features.v1.ShareTransactions.DeleteShareTransaction;

public record DeleteShareTransactionCommand(Guid Id) : ICommand;

public class DeleteShareTransactionHandler(MicrofinanceDbContext context) : ICommandHandler<DeleteShareTransactionCommand>
{
    public async ValueTask<Unit> Handle(DeleteShareTransactionCommand command, CancellationToken ct)
    {
        var entity = await context.ShareTransactions.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("ShareTransaction not found");
        
        context.ShareTransactions.Remove(entity);
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
