using FSH.Framework.Core.Exceptions;
using FSH.Modules.Microfinance.Data;

namespace FSH.Modules.Microfinance.Features.v1.DebtSettlements.DeleteDebtSettlement;

public record DeleteDebtSettlementCommand(Guid Id) : ICommand;

public class DeleteDebtSettlementHandler(MicrofinanceDbContext context) : ICommandHandler<DeleteDebtSettlementCommand>
{
    public async ValueTask<Unit> Handle(DeleteDebtSettlementCommand command, CancellationToken ct)
    {
        var entity = await context.DebtSettlements.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("DebtSettlement not found");
        
        context.DebtSettlements.Remove(entity);
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
