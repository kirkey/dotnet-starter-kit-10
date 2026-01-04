using FSH.Framework.Core.Context;
using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;
using FSH.Module.Microfinance.Domain;

namespace FSH.Module.Microfinance.Features.v1.LoanCollaterals.CreateLoanCollateral;

public record CreateLoanCollateralCommand(string Name) : ICommand<Guid>;

public class CreateLoanCollateralHandler(ICurrentUser currentUser,
    MicrofinanceDbContext context) : ICommandHandler<CreateLoanCollateralCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateLoanCollateralCommand command, CancellationToken ct)
    {
        var entity = LoanCollateral.Create(
            command.Name,
            currentUser.GetTenant() ?? "root",
            currentUser.GetUserId(),
            currentUser.Name ?? "System");
        
        context.LoanCollaterals.Add(entity);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
