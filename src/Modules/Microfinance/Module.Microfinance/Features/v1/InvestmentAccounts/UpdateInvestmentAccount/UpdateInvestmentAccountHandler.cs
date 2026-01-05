using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.InvestmentAccounts.UpdateInvestmentAccount;

namespace FSH.Module.Microfinance.Features.v1.InvestmentAccounts.UpdateInvestmentAccount;

public class UpdateInvestmentAccountHandler(MicrofinanceDbContext context) : ICommandHandler<UpdateInvestmentAccountCommand, Guid>
{
    public async ValueTask<Guid> Handle(UpdateInvestmentAccountCommand command, CancellationToken ct)
    {
        var entity = await context.InvestmentAccounts.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("InvestmentAccount not found");
        
        entity.Update(command.Name);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
