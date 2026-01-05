using FSH.Framework.Core.Context;
using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;
using FSH.Module.Microfinance.Domain;

using FSH.Module.Microfinance.Contracts.v1.CollateralValuations.CreateCollateralValuation;

namespace FSH.Module.Microfinance.Features.v1.CollateralValuations.CreateCollateralValuation;

public class CreateCollateralValuationHandler(ICurrentUser currentUser,
    MicrofinanceDbContext context) : ICommandHandler<CreateCollateralValuationCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateCollateralValuationCommand command, CancellationToken ct)
    {
        var entity = CollateralValuation.Create(
            command.Name,
            currentUser.GetTenant() ?? "root",
            currentUser.GetUserId(),
            currentUser.Name ?? "System");
        
        context.CollateralValuations.Add(entity);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
