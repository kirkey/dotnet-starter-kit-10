using FSH.Framework.Core.Exceptions;
using FSH.Modules.Microfinance.Data;

namespace FSH.Modules.Microfinance.Features.v1.CollateralTypes.UpdateCollateralType;

public record UpdateCollateralTypeCommand(Guid Id, string Name) : ICommand<Guid>;

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
