using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Contracts.v1.Meters;
using FSH.Module.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

using FSH.Module.Accounting.Contracts.v1.Meters.GetMeter;

namespace FSH.Module.Accounting.Features.v1.Meters.GetMeter;

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
