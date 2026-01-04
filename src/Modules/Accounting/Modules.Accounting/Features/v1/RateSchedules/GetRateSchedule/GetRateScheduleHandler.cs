using FSH.Framework.Core.Exceptions;
using FSH.Modules.Accounting.Contracts.v1.RateSchedules;
using FSH.Modules.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Modules.Accounting.Features.v1.RateSchedules.GetRateSchedule;

public record GetRateScheduleQuery(Guid Id) : IQuery<RateScheduleDto>;

public class GetRateScheduleHandler(AccountingDbContext context) : IQueryHandler<GetRateScheduleQuery, RateScheduleDto>
{
    public async ValueTask<RateScheduleDto> Handle(GetRateScheduleQuery query, CancellationToken ct)
    {
        var entity = await context.RateSchedules
            .Where(x => x.Id == query.Id)
            .Select(x => new RateScheduleDto(
                x.Id,
                x.Name,
                x.Description,
                x.IsActive,
                x.CreatedOnUtc))
            .FirstOrDefaultAsync(ct);
        
        return entity ?? throw new NotFoundException("RateSchedule not found");
    }
}
