using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.CollateralInsurances.DeleteCollateralInsurance;

namespace FSH.Module.Microfinance.Features.v1.CollateralInsurances.DeleteCollateralInsurance;

public class DeleteCollateralInsuranceHandler(MicrofinanceDbContext context) : ICommandHandler<DeleteCollateralInsuranceCommand>
{
    public async ValueTask<Unit> Handle(DeleteCollateralInsuranceCommand command, CancellationToken ct)
    {
        var entity = await context.CollateralInsurances.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("CollateralInsurance not found");
        
        context.CollateralInsurances.Remove(entity);
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
