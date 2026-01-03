using FSH.Framework.Core.Exceptions;
using FSH.Modules.Microfinance.Data;

namespace FSH.Modules.Microfinance.Features.v1.SavingsTransactions.DeleteSavingsTransaction;

public record DeleteSavingsTransactionCommand(Guid Id) : ICommand;

public class DeleteSavingsTransactionHandler(MicrofinanceDbContext context) : ICommandHandler<DeleteSavingsTransactionCommand>
{
    public async ValueTask<Unit> Handle(DeleteSavingsTransactionCommand command, CancellationToken ct)
    {
        var entity = await context.SavingsTransactions.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("SavingsTransaction not found");
        
        context.SavingsTransactions.Remove(entity);
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
