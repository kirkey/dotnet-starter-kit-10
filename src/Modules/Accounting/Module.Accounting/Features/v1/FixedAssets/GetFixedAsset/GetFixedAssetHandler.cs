using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Contracts.v1.FixedAssets;
using FSH.Module.Accounting.Contracts.v1.FixedAssets.GetFixedAsset;
using FSH.Module.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Module.Accounting.Features.v1.FixedAssets.GetFixedAsset;

public class GetFixedAssetHandler(AccountingDbContext context) : IQueryHandler<GetFixedAssetQuery, FixedAssetDto>
{
    public async ValueTask<FixedAssetDto> Handle(GetFixedAssetQuery query, CancellationToken ct)
    {
        var entity = await context.FixedAssets
            .Where(x => x.Id == query.Id)
            .Select(x => new FixedAssetDto(
                x.Id,
                x.Name,
                x.Description,
                x.AcquisitionDate,
                x.Cost,
                x.ResidualValue,
                x.DepreciationRate,
                x.AccumulatedDepreciation,
                x.IsDisposed,
                x.DisposalDate,
                x.DisposalProceeds,
                x.IsActive,
                x.CreatedOnUtc))
            .FirstOrDefaultAsync(ct);
        
        return entity ?? throw new NotFoundException("FixedAsset not found");
    }
}
