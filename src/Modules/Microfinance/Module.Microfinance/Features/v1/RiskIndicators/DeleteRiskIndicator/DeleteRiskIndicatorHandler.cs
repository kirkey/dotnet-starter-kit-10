using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

namespace FSH.Module.Microfinance.Features.v1.RiskIndicators.DeleteRiskIndicator;

public record DeleteRiskIndicatorCommand(Guid Id) : ICommand;

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
