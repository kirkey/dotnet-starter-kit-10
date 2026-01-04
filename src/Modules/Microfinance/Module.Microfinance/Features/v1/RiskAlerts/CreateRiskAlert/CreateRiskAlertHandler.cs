using FSH.Framework.Core.Context;
using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;
using FSH.Module.Microfinance.Domain;

namespace FSH.Module.Microfinance.Features.v1.RiskAlerts.CreateRiskAlert;

public record CreateRiskAlertCommand(string Name) : ICommand<Guid>;

public class CreateRiskAlertHandler(ICurrentUser currentUser,
    MicrofinanceDbContext context) : ICommandHandler<CreateRiskAlertCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateRiskAlertCommand command, CancellationToken ct)
    {
        var entity = RiskAlert.Create(
            command.Name,
            currentUser.GetTenant() ?? "root",
            currentUser.GetUserId(),
            currentUser.Name ?? "System");
        
        context.RiskAlerts.Add(entity);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
