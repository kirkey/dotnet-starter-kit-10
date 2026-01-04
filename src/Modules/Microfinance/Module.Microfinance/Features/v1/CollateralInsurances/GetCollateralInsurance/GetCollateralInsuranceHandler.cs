using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Contracts.v1.CollateralInsurances;
using FSH.Module.Microfinance.Data;

namespace FSH.Module.Microfinance.Features.v1.CollateralInsurances.GetCollateralInsurance;

public record GetCollateralInsuranceQuery(Guid Id) : IQuery<CollateralInsuranceDto>;

public class GetCollateralInsuranceHandler(MicrofinanceDbContext context) : IQueryHandler<GetCollateralInsuranceQuery, CollateralInsuranceDto>
{
    public async ValueTask<CollateralInsuranceDto> Handle(GetCollateralInsuranceQuery query, CancellationToken ct)
    {
        var entity = await context.CollateralInsurances
            .Where(x => x.Id == query.Id)
            .Select(x => new CollateralInsuranceDto(x.Id, x.Name, x.IsActive, x.CreatedOnUtc))
            .FirstOrDefaultAsync(ct);
        
        return entity ?? throw new NotFoundException("CollateralInsurance not found");
    }
}
