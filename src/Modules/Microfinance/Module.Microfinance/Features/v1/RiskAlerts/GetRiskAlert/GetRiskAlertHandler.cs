using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.RiskAlerts.GetRiskAlert;
using FSH.Module.Microfinance.Contracts.v1.RiskAlerts;

namespace FSH.Module.Microfinance.Features.v1.RiskAlerts.GetRiskAlert;

public class GetRiskAlertHandler(MicrofinanceDbContext context) : IQueryHandler<GetRiskAlertQuery, RiskAlertDto>
{
    public async ValueTask<RiskAlertDto> Handle(GetRiskAlertQuery query, CancellationToken ct)
    {
        var entity = await context.RiskAlerts
            .Where(x => x.Id == query.Id)
            .Select(x => new RiskAlertDto(x.Id, x.Name, x.IsActive, x.CreatedOnUtc))
            .FirstOrDefaultAsync(ct);
        
        return entity ?? throw new NotFoundException("RiskAlert not found");
    }
}
