using FSH.Framework.Core.Exceptions;
using FSH.Modules.Accounting.Contracts.v1.Meters;
using FSH.Modules.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Modules.Accounting.Features.v1.Meters.GetMeter;

public record GetMeterQuery(Guid Id) : IQuery<MeterDto>;

public class GetMeterHandler(AccountingDbContext context) : IQueryHandler<GetMeterQuery, MeterDto>
{
    public async ValueTask<MeterDto> Handle(GetMeterQuery query, CancellationToken ct)
    {
        var entity = await context.Meters
            .Where(x => x.Id == query.Id)
            .Select(x => new MeterDto(
                x.Id,
                x.Name,
                x.Description,
                x.IsActive,
                x.CreatedOnUtc))
            .FirstOrDefaultAsync(ct);
        
        return entity ?? throw new NotFoundException("Meter not found");
    }
}
