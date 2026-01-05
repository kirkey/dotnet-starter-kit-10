using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.InvestmentTransactions.DeleteInvestmentTransaction;

namespace FSH.Module.Microfinance.Features.v1.InvestmentTransactions.DeleteInvestmentTransaction;

public class DeleteInvestmentTransactionHandler(MicrofinanceDbContext context) : ICommandHandler<DeleteInvestmentTransactionCommand>
{
    public async ValueTask<Unit> Handle(DeleteInvestmentTransactionCommand command, CancellationToken ct)
    {
        var entity = await context.InvestmentTransactions.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("InvestmentTransaction not found");
        
        context.InvestmentTransactions.Remove(entity);
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
