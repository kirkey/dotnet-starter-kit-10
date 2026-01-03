using FSH.Framework.Core.Context;
using FSH.Framework.Core.Exceptions;
using FSH.Modules.Microfinance.Data;
using FSH.Modules.Microfinance.Domain;

namespace FSH.Modules.Microfinance.Features.v1.CollateralReleases.CreateCollateralRelease;

public record CreateCollateralReleaseCommand(string Name) : ICommand<Guid>;

public class CreateCollateralReleaseHandler(ICurrentUser currentUser,
    MicrofinanceDbContext context) : ICommandHandler<CreateCollateralReleaseCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateCollateralReleaseCommand command, CancellationToken ct)
    {
        var entity = CollateralRelease.Create(
            command.Name,
            currentUser.GetTenant() ?? "root",
            currentUser.GetUserId(),
            currentUser.Name ?? "System");
        
        context.CollateralReleases.Add(entity);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
