using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.RiskIndicators.UpdateRiskIndicator;

namespace FSH.Module.Microfinance.Features.v1.RiskIndicators.UpdateRiskIndicator;

public class UpdateRiskIndicatorHandler(MicrofinanceDbContext context) : ICommandHandler<UpdateRiskIndicatorCommand, Guid>
{
    public async ValueTask<Guid> Handle(UpdateRiskIndicatorCommand command, CancellationToken ct)
    {
        var entity = await context.RiskIndicators.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("RiskIndicator not found");
        
        entity.Update(command.Name);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
