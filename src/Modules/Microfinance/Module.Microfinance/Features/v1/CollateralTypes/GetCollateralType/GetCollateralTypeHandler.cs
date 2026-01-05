using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Contracts.v1.CollateralTypes;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.CollateralTypes.GetCollateralType;

namespace FSH.Module.Microfinance.Features.v1.CollateralTypes.GetCollateralType;

public class GetCollateralTypeHandler(MicrofinanceDbContext context) : IQueryHandler<GetCollateralTypeQuery, CollateralTypeDto>
{
    public async ValueTask<CollateralTypeDto> Handle(GetCollateralTypeQuery query, CancellationToken ct)
    {
        var entity = await context.CollateralTypes
            .Where(x => x.Id == query.Id)
            .Select(x => new CollateralTypeDto(x.Id, x.Name, x.IsActive, x.CreatedOnUtc))
            .FirstOrDefaultAsync(ct);
        
        return entity ?? throw new NotFoundException("CollateralType not found");
    }
}
