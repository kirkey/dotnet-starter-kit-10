using FSH.Framework.Core.Context;
using FSH.Framework.Core.Exceptions;
using FSH.Modules.Microfinance.Data;
using FSH.Modules.Microfinance.Domain;

namespace FSH.Modules.Microfinance.Features.v1.RiskIndicators.CreateRiskIndicator;

public record CreateRiskIndicatorCommand(string Name) : ICommand<Guid>;

public class CreateRiskIndicatorHandler(ICurrentUser currentUser,
    MicrofinanceDbContext context) : ICommandHandler<CreateRiskIndicatorCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateRiskIndicatorCommand command, CancellationToken ct)
    {
        var entity = RiskIndicator.Create(
            command.Name,
            currentUser.GetTenant() ?? "root",
            currentUser.GetUserId(),
            currentUser.Name ?? "System");
        
        context.RiskIndicators.Add(entity);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
