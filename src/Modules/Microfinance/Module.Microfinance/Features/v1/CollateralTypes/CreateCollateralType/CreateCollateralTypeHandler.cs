using FSH.Framework.Core.Context;
using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;
using FSH.Module.Microfinance.Domain;

namespace FSH.Module.Microfinance.Features.v1.CollateralTypes.CreateCollateralType;

public record CreateCollateralTypeCommand(string Name) : ICommand<Guid>;

public class CreateCollateralTypeHandler(ICurrentUser currentUser,
    MicrofinanceDbContext context) : ICommandHandler<CreateCollateralTypeCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateCollateralTypeCommand command, CancellationToken ct)
    {
        var entity = CollateralType.Create(
            command.Name,
            currentUser.GetTenant() ?? "root",
            currentUser.GetUserId(),
            currentUser.Name ?? "System");
        
        context.CollateralTypes.Add(entity);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
