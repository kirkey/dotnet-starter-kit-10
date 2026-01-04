using FSH.Framework.Core.Context;
using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Contracts.v1.FixedAssets.DepreciateFixedAsset;
using FSH.Module.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Module.Accounting.Features.v1.FixedAssets.DepreciateFixedAsset;

public class DepreciateFixedAssetHandler(AccountingDbContext context, ICurrentUser currentUser) 
    : ICommandHandler<DepreciateFixedAssetCommand>
{
    public async ValueTask<Unit> Handle(DepreciateFixedAssetCommand command, CancellationToken ct)
    {
        var entity = await context.FixedAssets.FirstOrDefaultAsync(x => x.Id == command.Id, ct)
            ?? throw new NotFoundException("FixedAsset not found");

        if (entity.IsDisposed)
            throw new BadRequestException("Cannot depreciate a disposed asset");

        // Determine amount: use provided amount or compute from rate
        decimal amount = command.Amount ?? Math.Round(entity.Cost * entity.DepreciationRate, 2);

        entity.Depreciate(amount);

        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
