using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.CollateralTypes.UpdateCollateralType;

namespace FSH.Module.Microfinance.Features.v1.CollateralTypes.UpdateCollateralType;

public class UpdateCollateralTypeHandler(MicrofinanceDbContext context) : ICommandHandler<UpdateCollateralTypeCommand, Guid>
{
    public async ValueTask<Guid> Handle(UpdateCollateralTypeCommand command, CancellationToken ct)
    {
        var entity = await context.CollateralTypes.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("CollateralType not found");
        
        entity.Update(command.Name);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
