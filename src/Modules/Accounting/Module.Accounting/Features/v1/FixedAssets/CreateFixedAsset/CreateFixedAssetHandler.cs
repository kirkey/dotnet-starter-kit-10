using FSH.Framework.Shared.Identity;
using FSH.Module.Accounting.Contracts.v1.FixedAssets.CreateFixedAsset;
using FSH.Module.Accounting.Data;
using FSH.Module.Accounting.Domain;
using Mediator;

namespace FSH.Module.Accounting.Features.v1.FixedAssets.CreateFixedAsset;

public class CreateFixedAssetHandler(AccountingDbContext context, ICurrentUser currentUser) 
    : ICommandHandler<CreateFixedAssetCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateFixedAssetCommand command, CancellationToken ct)
    {
        var entity = FixedAsset.Create(
            command.Name,
            currentUser.GetTenant() ?? "root",
            currentUser.GetUserId(),
            currentUser.Name ?? "System",
            command.Description,
            command.AcquisitionDate,
            command.Cost,
            command.ResidualValue,
            command.DepreciationRate);
        
        context.FixedAssets.Add(entity);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
