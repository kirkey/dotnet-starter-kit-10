using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

namespace FSH.Module.Microfinance.Features.v1.LoanCollaterals.DeleteLoanCollateral;

public record DeleteLoanCollateralCommand(Guid Id) : ICommand;

public class DeleteLoanCollateralHandler(MicrofinanceDbContext context) : ICommandHandler<DeleteLoanCollateralCommand>
{
    public async ValueTask<Unit> Handle(DeleteLoanCollateralCommand command, CancellationToken ct)
    {
        var entity = await context.LoanCollaterals.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("LoanCollateral not found");
        
        context.LoanCollaterals.Remove(entity);
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
