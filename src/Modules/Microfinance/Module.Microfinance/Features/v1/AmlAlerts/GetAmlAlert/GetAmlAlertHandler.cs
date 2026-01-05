using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.AmlAlerts.GetAmlAlert;
using FSH.Module.Microfinance.Contracts.v1.AmlAlerts;

namespace FSH.Module.Microfinance.Features.v1.AmlAlerts.GetAmlAlert;

public class GetAmlAlertHandler(MicrofinanceDbContext context) : IQueryHandler<GetAmlAlertQuery, AmlAlertDto>
{
    public async ValueTask<AmlAlertDto> Handle(GetAmlAlertQuery query, CancellationToken ct)
    {
        var entity = await context.AmlAlerts
            .Where(x => x.Id == query.Id)
            .Select(x => new AmlAlertDto(x.Id, x.Name, x.IsActive, x.CreatedOnUtc))
            .FirstOrDefaultAsync(ct);
        
        return entity ?? throw new NotFoundException("AmlAlert not found");
    }
}
