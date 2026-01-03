using FSH.Framework.Core.Exceptions;
using FSH.Modules.Microfinance.Contracts.v1.RiskIndicators;
using FSH.Modules.Microfinance.Data;

namespace FSH.Modules.Microfinance.Features.v1.RiskIndicators.GetRiskIndicator;

public record GetRiskIndicatorQuery(Guid Id) : IQuery<RiskIndicatorDto>;

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
