using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.CollateralValuations.GetCollateralValuation;
using FSH.Module.Microfinance.Contracts.v1.CollateralValuations;

namespace FSH.Module.Microfinance.Features.v1.CollateralValuations.GetCollateralValuation;

public class GetCollateralValuationHandler(MicrofinanceDbContext context) : IQueryHandler<GetCollateralValuationQuery, CollateralValuationDto>
{
    public async ValueTask<CollateralValuationDto> Handle(GetCollateralValuationQuery query, CancellationToken ct)
    {
        var entity = await context.CollateralValuations
            .Where(x => x.Id == query.Id)
            .Select(x => new CollateralValuationDto(x.Id, x.Name, x.IsActive, x.CreatedOnUtc))
            .FirstOrDefaultAsync(ct);
        
        return entity ?? throw new NotFoundException("CollateralValuation not found");
    }
}
