using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.ShareTransactions.UpdateShareTransaction;

namespace FSH.Module.Microfinance.Features.v1.ShareTransactions.UpdateShareTransaction;

public class UpdateShareTransactionHandler(MicrofinanceDbContext context) : ICommandHandler<UpdateShareTransactionCommand, Guid>
{
    public async ValueTask<Guid> Handle(UpdateShareTransactionCommand command, CancellationToken ct)
    {
        var entity = await context.ShareTransactions.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("ShareTransaction not found");
        
        entity.Update(command.Name);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
