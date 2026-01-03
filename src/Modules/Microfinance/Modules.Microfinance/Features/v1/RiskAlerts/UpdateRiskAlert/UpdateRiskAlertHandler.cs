using FSH.Framework.Core.Exceptions;
using FSH.Modules.Microfinance.Data;

namespace FSH.Modules.Microfinance.Features.v1.RiskAlerts.UpdateRiskAlert;

public record UpdateRiskAlertCommand(Guid Id, string Name) : ICommand<Guid>;

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
