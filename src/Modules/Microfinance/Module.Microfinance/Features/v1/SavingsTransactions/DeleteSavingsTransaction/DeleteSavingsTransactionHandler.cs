using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.SavingsTransactions.DeleteSavingsTransaction;

namespace FSH.Module.Microfinance.Features.v1.SavingsTransactions.DeleteSavingsTransaction;

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
