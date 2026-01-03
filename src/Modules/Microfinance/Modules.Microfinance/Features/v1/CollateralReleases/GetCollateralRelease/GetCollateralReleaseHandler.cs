using FSH.Framework.Core.Exceptions;
using FSH.Modules.Microfinance.Contracts.v1.CollateralReleases;
using FSH.Modules.Microfinance.Data;

namespace FSH.Modules.Microfinance.Features.v1.CollateralReleases.GetCollateralRelease;

public record GetCollateralReleaseQuery(Guid Id) : IQuery<CollateralReleaseDto>;

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
