using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Contracts.v1.FixedAssets.DeleteFixedAsset;
using FSH.Module.Accounting.Data;
using Mediator;

namespace FSH.Module.Accounting.Features.v1.FixedAssets.DeleteFixedAsset;

public class DeleteFixedAssetHandler(AccountingDbContext context) : ICommandHandler<DeleteFixedAssetCommand>
{
    public async ValueTask<Unit> Handle(DeleteFixedAssetCommand command, CancellationToken ct)
    {
        var entity = await context.FixedAssets.FindAsync(command.Id, ct)
            ?? throw new NotFoundException("FixedAsset not found");
        
        context.FixedAssets.Remove(entity);
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
