using FSH.Framework.Core.Exceptions;
using FSH.Modules.Microfinance.Data;

namespace FSH.Modules.Microfinance.Features.v1.RiskIndicators.UpdateRiskIndicator;

public record UpdateRiskIndicatorCommand(Guid Id, string Name) : ICommand<Guid>;

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
