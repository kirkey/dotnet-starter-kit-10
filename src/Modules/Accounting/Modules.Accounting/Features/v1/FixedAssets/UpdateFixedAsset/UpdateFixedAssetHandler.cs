using FSH.Framework.Core.Exceptions;
using FSH.Modules.Accounting.Data;
using Mediator;

namespace FSH.Modules.Accounting.Features.v1.FixedAssets.UpdateFixedAsset;

public record UpdateFixedAssetCommand(Guid Id, string Name, string? Description = null, DateTime? AcquisitionDate = null, decimal? Cost = null, decimal? ResidualValue = null, decimal? DepreciationRate = null) : ICommand<Guid>;

public class UpdateFixedAssetHandler(AccountingDbContext context) : ICommandHandler<UpdateFixedAssetCommand, Guid>
{
    public async ValueTask<Guid> Handle(UpdateFixedAssetCommand command, CancellationToken ct)
    {
        var entity = await context.FixedAssets.FindAsync(command.Id, ct)
            ?? throw new NotFoundException("FixedAsset not found");
        
        entity.Update(command.Name, command.Description, command.AcquisitionDate, command.Cost, command.ResidualValue, command.DepreciationRate);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
