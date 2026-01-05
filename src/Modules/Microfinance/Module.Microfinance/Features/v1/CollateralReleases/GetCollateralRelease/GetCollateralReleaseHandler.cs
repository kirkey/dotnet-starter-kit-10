using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Contracts.v1.CollateralReleases;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.CollateralReleases.GetCollateralRelease;

namespace FSH.Module.Microfinance.Features.v1.CollateralReleases.GetCollateralRelease;

public class GetCollateralReleaseHandler(MicrofinanceDbContext context) : IQueryHandler<GetCollateralReleaseQuery, CollateralReleaseDto>
{
    public async ValueTask<CollateralReleaseDto> Handle(GetCollateralReleaseQuery query, CancellationToken ct)
    {
        var entity = await context.CollateralReleases
            .Where(x => x.Id == query.Id)
            .Select(x => new CollateralReleaseDto(x.Id, x.Name, x.IsActive, x.CreatedOnUtc))
            .FirstOrDefaultAsync(ct);
        
        return entity ?? throw new NotFoundException("CollateralRelease not found");
    }
}
