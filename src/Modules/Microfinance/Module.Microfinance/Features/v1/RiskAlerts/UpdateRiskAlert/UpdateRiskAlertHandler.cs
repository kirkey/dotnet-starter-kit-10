using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.RiskAlerts.UpdateRiskAlert;

namespace FSH.Module.Microfinance.Features.v1.RiskAlerts.UpdateRiskAlert;

public class UpdateRiskAlertHandler(MicrofinanceDbContext context) : ICommandHandler<UpdateRiskAlertCommand, Guid>
{
    public async ValueTask<Guid> Handle(UpdateRiskAlertCommand command, CancellationToken ct)
    {
        var entity = await context.RiskAlerts.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("RiskAlert not found");
        
        entity.Update(command.Name);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
