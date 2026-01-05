using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.LoanCollaterals.UpdateLoanCollateral;

namespace FSH.Module.Microfinance.Features.v1.LoanCollaterals.UpdateLoanCollateral;

public class UpdateLoanCollateralHandler(MicrofinanceDbContext context) : ICommandHandler<UpdateLoanCollateralCommand, Guid>
{
    public async ValueTask<Guid> Handle(UpdateLoanCollateralCommand command, CancellationToken ct)
    {
        var entity = await context.LoanCollaterals.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("LoanCollateral not found");
        
        entity.Update(command.Name);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
