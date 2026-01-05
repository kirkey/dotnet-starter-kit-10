using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.SavingsTransactions.UpdateSavingsTransaction;

namespace FSH.Module.Microfinance.Features.v1.SavingsTransactions.UpdateSavingsTransaction;

public class UpdateSavingsTransactionHandler(MicrofinanceDbContext context) : ICommandHandler<UpdateSavingsTransactionCommand, Guid>
{
    public async ValueTask<Guid> Handle(UpdateSavingsTransactionCommand command, CancellationToken ct)
    {
        var entity = await context.SavingsTransactions.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("SavingsTransaction not found");
        
        entity.Update(command.Name);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
