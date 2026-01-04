using FSH.Framework.Core.Exceptions;
using FSH.Modules.Accounting.Data;
using Mediator;

namespace FSH.Modules.Accounting.Features.v1.FixedAssets.DeleteFixedAsset;

public record DeleteFixedAssetCommand(Guid Id) : ICommand;

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
