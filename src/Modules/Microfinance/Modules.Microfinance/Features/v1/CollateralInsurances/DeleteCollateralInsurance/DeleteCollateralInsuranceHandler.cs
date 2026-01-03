using FSH.Framework.Core.Exceptions;
using FSH.Modules.Microfinance.Data;

namespace FSH.Modules.Microfinance.Features.v1.CollateralInsurances.DeleteCollateralInsurance;

public record DeleteCollateralInsuranceCommand(Guid Id) : ICommand;

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
