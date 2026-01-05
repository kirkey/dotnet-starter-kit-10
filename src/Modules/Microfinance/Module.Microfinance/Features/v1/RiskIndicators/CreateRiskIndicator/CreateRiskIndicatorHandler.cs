using FSH.Framework.Core.Context;
using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;
using FSH.Module.Microfinance.Domain;

using FSH.Module.Microfinance.Contracts.v1.RiskIndicators.CreateRiskIndicator;

namespace FSH.Module.Microfinance.Features.v1.RiskIndicators.CreateRiskIndicator;

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
