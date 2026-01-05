using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.InvestmentTransactions.UpdateInvestmentTransaction;

namespace FSH.Module.Microfinance.Features.v1.InvestmentTransactions.UpdateInvestmentTransaction;

public class UpdateInvestmentTransactionHandler(MicrofinanceDbContext context) : ICommandHandler<UpdateInvestmentTransactionCommand, Guid>
{
    public async ValueTask<Guid> Handle(UpdateInvestmentTransactionCommand command, CancellationToken ct)
    {
        var entity = await context.InvestmentTransactions.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("InvestmentTransaction not found");
        
        entity.Update(command.Name);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
