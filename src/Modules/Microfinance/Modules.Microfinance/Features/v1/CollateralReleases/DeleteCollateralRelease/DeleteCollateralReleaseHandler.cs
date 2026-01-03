using FSH.Framework.Core.Exceptions;
using FSH.Modules.Microfinance.Data;

namespace FSH.Modules.Microfinance.Features.v1.CollateralReleases.DeleteCollateralRelease;

public record DeleteCollateralReleaseCommand(Guid Id) : ICommand;

public class DeleteCollateralReleaseHandler(MicrofinanceDbContext context) : ICommandHandler<DeleteCollateralReleaseCommand>
{
    public async ValueTask<Unit> Handle(DeleteCollateralReleaseCommand command, CancellationToken ct)
    {
        var entity = await context.CollateralReleases.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("CollateralRelease not found");
        
        context.CollateralReleases.Remove(entity);
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
