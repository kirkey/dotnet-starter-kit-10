using FSH.Framework.Core.Context;
using FSH.Framework.Core.Exceptions;
using FSH.Modules.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Modules.Accounting.Features.v1.FixedAssets.DisposeFixedAsset;

public record DisposeFixedAssetCommand(Guid Id, DateTime? DisposalDate = null, decimal? Proceeds = null) : ICommand;

public class DisposeFixedAssetHandler(AccountingDbContext context, ICurrentUser currentUser) 
    : ICommandHandler<DisposeFixedAssetCommand>
{
    public async ValueTask<Unit> Handle(DisposeFixedAssetCommand command, CancellationToken ct)
    {
        var entity = await context.FixedAssets.FirstOrDefaultAsync(x => x.Id == command.Id, ct)
            ?? throw new NotFoundException("FixedAsset not found");

        if (entity.IsDisposed)
            throw new BadRequestException("Asset already disposed");

        var date = command.DisposalDate ?? DateTime.UtcNow.Date;
        entity.DisposeAsset(date, command.Proceeds);

        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
