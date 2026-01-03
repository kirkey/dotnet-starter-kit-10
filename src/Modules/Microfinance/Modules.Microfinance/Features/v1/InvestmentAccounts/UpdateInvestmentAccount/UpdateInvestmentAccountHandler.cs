using FSH.Framework.Core.Exceptions;
using FSH.Modules.Microfinance.Data;

namespace FSH.Modules.Microfinance.Features.v1.InvestmentAccounts.UpdateInvestmentAccount;

public record UpdateInvestmentAccountCommand(Guid Id, string Name) : ICommand<Guid>;

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
