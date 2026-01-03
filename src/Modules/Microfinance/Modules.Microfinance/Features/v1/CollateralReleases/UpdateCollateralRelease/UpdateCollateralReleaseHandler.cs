using FSH.Framework.Core.Exceptions;
using FSH.Modules.Microfinance.Data;

namespace FSH.Modules.Microfinance.Features.v1.CollateralReleases.UpdateCollateralRelease;

public record UpdateCollateralReleaseCommand(Guid Id, string Name) : ICommand<Guid>;

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
