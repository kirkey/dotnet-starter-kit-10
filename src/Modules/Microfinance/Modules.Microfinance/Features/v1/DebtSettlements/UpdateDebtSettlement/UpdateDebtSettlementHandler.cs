using FSH.Framework.Core.Exceptions;
using FSH.Modules.Microfinance.Data;

namespace FSH.Modules.Microfinance.Features.v1.DebtSettlements.UpdateDebtSettlement;

public record UpdateDebtSettlementCommand(Guid Id, string Name) : ICommand<Guid>;

public class UpdateDebtSettlementHandler(MicrofinanceDbContext context) : ICommandHandler<UpdateDebtSettlementCommand, Guid>
{
    public async ValueTask<Guid> Handle(UpdateDebtSettlementCommand command, CancellationToken ct)
    {
        var entity = await context.DebtSettlements.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("DebtSettlement not found");
        
        entity.Update(command.Name);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
