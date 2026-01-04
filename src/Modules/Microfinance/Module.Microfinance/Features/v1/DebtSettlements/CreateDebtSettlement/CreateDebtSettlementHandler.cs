using FSH.Framework.Core.Context;
using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;
using FSH.Module.Microfinance.Domain;

namespace FSH.Module.Microfinance.Features.v1.DebtSettlements.CreateDebtSettlement;

public record CreateDebtSettlementCommand(string Name) : ICommand<Guid>;

public class CreateDebtSettlementHandler(ICurrentUser currentUser,
    MicrofinanceDbContext context) : ICommandHandler<CreateDebtSettlementCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateDebtSettlementCommand command, CancellationToken ct)
    {
        var entity = DebtSettlement.Create(
            command.Name,
            currentUser.GetTenant() ?? "root",
            currentUser.GetUserId(),
            currentUser.Name ?? "System");
        
        context.DebtSettlements.Add(entity);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
