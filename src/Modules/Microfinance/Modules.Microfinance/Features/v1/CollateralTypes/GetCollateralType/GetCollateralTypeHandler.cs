using FSH.Framework.Core.Exceptions;
using FSH.Modules.Microfinance.Contracts.v1.CollateralTypes;
using FSH.Modules.Microfinance.Data;

namespace FSH.Modules.Microfinance.Features.v1.CollateralTypes.GetCollateralType;

public record GetCollateralTypeQuery(Guid Id) : IQuery<CollateralTypeDto>;

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
