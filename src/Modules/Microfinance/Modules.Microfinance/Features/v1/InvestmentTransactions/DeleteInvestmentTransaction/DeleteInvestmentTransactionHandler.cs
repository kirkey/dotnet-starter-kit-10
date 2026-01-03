using FSH.Framework.Core.Exceptions;
using FSH.Modules.Microfinance.Data;

namespace FSH.Modules.Microfinance.Features.v1.InvestmentTransactions.DeleteInvestmentTransaction;

public record DeleteInvestmentTransactionCommand(Guid Id) : ICommand;

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
