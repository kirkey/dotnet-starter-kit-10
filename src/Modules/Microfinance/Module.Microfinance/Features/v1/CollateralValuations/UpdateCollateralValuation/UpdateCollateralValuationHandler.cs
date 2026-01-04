using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

namespace FSH.Module.Microfinance.Features.v1.CollateralValuations.UpdateCollateralValuation;

public record UpdateCollateralValuationCommand(Guid Id, string Name) : ICommand<Guid>;

public class UpdateCollateralValuationHandler(MicrofinanceDbContext context) : ICommandHandler<UpdateCollateralValuationCommand, Guid>
{
    public async ValueTask<Guid> Handle(UpdateCollateralValuationCommand command, CancellationToken ct)
    {
        var entity = await context.CollateralValuations.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("CollateralValuation not found");
        
        entity.Update(command.Name);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
