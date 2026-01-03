using FSH.Framework.Core.Context;
using FSH.Framework.Core.Exceptions;
using FSH.Modules.Microfinance.Data;
using FSH.Modules.Microfinance.Domain;

namespace FSH.Modules.Microfinance.Features.v1.InvestmentAccounts.CreateInvestmentAccount;

public record CreateInvestmentAccountCommand(string Name) : ICommand<Guid>;

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
