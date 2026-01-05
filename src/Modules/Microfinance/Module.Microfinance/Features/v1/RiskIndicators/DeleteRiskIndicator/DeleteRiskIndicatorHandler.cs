using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.RiskIndicators.DeleteRiskIndicator;

namespace FSH.Module.Microfinance.Features.v1.RiskIndicators.DeleteRiskIndicator;

public class DeleteRiskIndicatorHandler(MicrofinanceDbContext context) : ICommandHandler<DeleteRiskIndicatorCommand>
{
    public async ValueTask<Unit> Handle(DeleteRiskIndicatorCommand command, CancellationToken ct)
    {
        var entity = await context.RiskIndicators.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("RiskIndicator not found");
        
        context.RiskIndicators.Remove(entity);
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
