using FSH.Framework.Core.Exceptions;
using FSH.Modules.Microfinance.Data;

namespace FSH.Modules.Microfinance.Features.v1.CollateralInsurances.UpdateCollateralInsurance;

public record UpdateCollateralInsuranceCommand(Guid Id, string Name) : ICommand<Guid>;

public class UpdateCollateralInsuranceHandler(MicrofinanceDbContext context) : ICommandHandler<UpdateCollateralInsuranceCommand, Guid>
{
    public async ValueTask<Guid> Handle(UpdateCollateralInsuranceCommand command, CancellationToken ct)
    {
        var entity = await context.CollateralInsurances.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("CollateralInsurance not found");
        
        entity.Update(command.Name);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
