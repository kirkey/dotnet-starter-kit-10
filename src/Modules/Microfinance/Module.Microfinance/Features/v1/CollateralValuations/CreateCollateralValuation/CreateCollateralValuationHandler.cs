using FSH.Framework.Core.Context;
using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;
using FSH.Module.Microfinance.Domain;

namespace FSH.Module.Microfinance.Features.v1.CollateralValuations.CreateCollateralValuation;

public record CreateCollateralValuationCommand(string Name) : ICommand<Guid>;

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
