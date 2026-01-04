using FSH.Framework.Core.Context;
using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;
using FSH.Module.Microfinance.Domain;

namespace FSH.Module.Microfinance.Features.v1.CollateralInsurances.CreateCollateralInsurance;

public record CreateCollateralInsuranceCommand(string Name) : ICommand<Guid>;

public class CreateCollateralInsuranceHandler(ICurrentUser currentUser,
    MicrofinanceDbContext context) : ICommandHandler<CreateCollateralInsuranceCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateCollateralInsuranceCommand command, CancellationToken ct)
    {
        var entity = CollateralInsurance.Create(
            command.Name,
            currentUser.GetTenant() ?? "root",
            currentUser.GetUserId(),
            currentUser.Name ?? "System");
        
        context.CollateralInsurances.Add(entity);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
