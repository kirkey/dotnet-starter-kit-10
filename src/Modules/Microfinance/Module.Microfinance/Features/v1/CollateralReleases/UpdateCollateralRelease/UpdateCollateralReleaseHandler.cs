using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.CollateralReleases.UpdateCollateralRelease;

namespace FSH.Module.Microfinance.Features.v1.CollateralReleases.UpdateCollateralRelease;

public class UpdateCollateralReleaseHandler(MicrofinanceDbContext context) : ICommandHandler<UpdateCollateralReleaseCommand, Guid>
{
    public async ValueTask<Guid> Handle(UpdateCollateralReleaseCommand command, CancellationToken ct)
    {
        var entity = await context.CollateralReleases.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("CollateralRelease not found");
        
        entity.Update(command.Name);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
