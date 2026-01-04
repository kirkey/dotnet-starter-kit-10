using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

namespace FSH.Module.Microfinance.Features.v1.InvestmentAccounts.DeleteInvestmentAccount;

public record DeleteInvestmentAccountCommand(Guid Id) : ICommand;

public class DeleteInvestmentAccountHandler(MicrofinanceDbContext context) : ICommandHandler<DeleteInvestmentAccountCommand>
{
    public async ValueTask<Unit> Handle(DeleteInvestmentAccountCommand command, CancellationToken ct)
    {
        var entity = await context.InvestmentAccounts.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("InvestmentAccount not found");
        
        context.InvestmentAccounts.Remove(entity);
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
