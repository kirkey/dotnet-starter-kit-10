using FSH.Framework.Core.Identity;
using FSH.Modules.Accounting.Data;
using FSH.Modules.Accounting.Domain;
using Mediator;

namespace FSH.Modules.Accounting.Features.v1.FixedAssets.CreateFixedAsset;

public record CreateFixedAssetCommand(
    string Name,
    string? Description = null,
    DateTime? AcquisitionDate = null,
    decimal Cost = 0m,
    decimal ResidualValue = 0m,
    decimal DepreciationRate = 0m) : ICommand<Guid>;

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
