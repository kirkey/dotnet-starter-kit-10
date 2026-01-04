using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Contracts.v1.CollateralValuations;
using FSH.Module.Microfinance.Data;

namespace FSH.Module.Microfinance.Features.v1.CollateralValuations.GetCollateralValuation;

public record GetCollateralValuationQuery(Guid Id) : IQuery<CollateralValuationDto>;

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
