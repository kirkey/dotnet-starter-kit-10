using FSH.Framework.Core.Exceptions;
using FSH.Modules.Microfinance.Data;

namespace FSH.Modules.Microfinance.Features.v1.RiskAlerts.DeleteRiskAlert;

public record DeleteRiskAlertCommand(Guid Id) : ICommand;

public class DeleteRiskAlertHandler(MicrofinanceDbContext context) : ICommandHandler<DeleteRiskAlertCommand>
{
    public async ValueTask<Unit> Handle(DeleteRiskAlertCommand command, CancellationToken ct)
    {
        var entity = await context.RiskAlerts.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("RiskAlert not found");
        
        context.RiskAlerts.Remove(entity);
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
