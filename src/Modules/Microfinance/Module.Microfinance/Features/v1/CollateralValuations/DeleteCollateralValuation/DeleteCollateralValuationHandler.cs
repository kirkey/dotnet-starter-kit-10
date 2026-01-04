using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

namespace FSH.Module.Microfinance.Features.v1.CollateralValuations.DeleteCollateralValuation;

public record DeleteCollateralValuationCommand(Guid Id) : ICommand;

public class DeleteCollateralValuationHandler(MicrofinanceDbContext context) : ICommandHandler<DeleteCollateralValuationCommand>
{
    public async ValueTask<Unit> Handle(DeleteCollateralValuationCommand command, CancellationToken ct)
    {
        var entity = await context.CollateralValuations.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("CollateralValuation not found");
        
        context.CollateralValuations.Remove(entity);
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
