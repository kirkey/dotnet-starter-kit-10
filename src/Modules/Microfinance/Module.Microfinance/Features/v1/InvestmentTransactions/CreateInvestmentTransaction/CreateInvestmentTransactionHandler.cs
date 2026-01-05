using FSH.Framework.Core.Context;
using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;
using FSH.Module.Microfinance.Domain;

using FSH.Module.Microfinance.Contracts.v1.InvestmentTransactions.CreateInvestmentTransaction;

namespace FSH.Module.Microfinance.Features.v1.InvestmentTransactions.CreateInvestmentTransaction;

public class CreateInvestmentTransactionHandler(ICurrentUser currentUser,
    MicrofinanceDbContext context) : ICommandHandler<CreateInvestmentTransactionCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateInvestmentTransactionCommand command, CancellationToken ct)
    {
        var entity = InvestmentTransaction.Create(
            command.Name,
            currentUser.GetTenant() ?? "root",
            currentUser.GetUserId(),
            currentUser.Name ?? "System");
        
        context.InvestmentTransactions.Add(entity);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
