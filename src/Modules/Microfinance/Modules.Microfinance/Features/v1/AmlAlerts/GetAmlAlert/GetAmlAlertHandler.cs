using FSH.Framework.Core.Exceptions;
using FSH.Modules.Microfinance.Contracts.v1.AmlAlerts;
using FSH.Modules.Microfinance.Data;

namespace FSH.Modules.Microfinance.Features.v1.AmlAlerts.GetAmlAlert;

public record GetAmlAlertQuery(Guid Id) : IQuery<AmlAlertDto>;

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
