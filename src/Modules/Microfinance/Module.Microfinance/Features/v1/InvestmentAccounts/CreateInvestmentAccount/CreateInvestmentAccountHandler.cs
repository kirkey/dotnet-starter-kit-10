using FSH.Framework.Core.Context;
using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;
using FSH.Module.Microfinance.Domain;

using FSH.Module.Microfinance.Contracts.v1.InvestmentAccounts.CreateInvestmentAccount;

namespace FSH.Module.Microfinance.Features.v1.InvestmentAccounts.CreateInvestmentAccount;

public class CreateInvestmentAccountHandler(ICurrentUser currentUser,
    MicrofinanceDbContext context) : ICommandHandler<CreateInvestmentAccountCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateInvestmentAccountCommand command, CancellationToken ct)
    {
        var entity = InvestmentAccount.Create(
            command.Name,
            currentUser.GetTenant() ?? "root",
            currentUser.GetUserId(),
            currentUser.Name ?? "System");
        
        context.InvestmentAccounts.Add(entity);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
