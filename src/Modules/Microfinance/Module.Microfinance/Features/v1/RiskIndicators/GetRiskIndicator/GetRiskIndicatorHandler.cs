using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Contracts.v1.RiskIndicators;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.RiskIndicators.GetRiskIndicator;

namespace FSH.Module.Microfinance.Features.v1.RiskIndicators.GetRiskIndicator;

public class GetRiskIndicatorHandler(MicrofinanceDbContext context) : IQueryHandler<GetRiskIndicatorQuery, RiskIndicatorDto>
{
    public async ValueTask<RiskIndicatorDto> Handle(GetRiskIndicatorQuery query, CancellationToken ct)
    {
        var entity = await context.RiskIndicators
            .Where(x => x.Id == query.Id)
            .Select(x => new RiskIndicatorDto(x.Id, x.Name, x.IsActive, x.CreatedOnUtc))
            .FirstOrDefaultAsync(ct);
        
        return entity ?? throw new NotFoundException("RiskIndicator not found");
    }
}
