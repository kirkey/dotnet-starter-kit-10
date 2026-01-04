using FSH.Framework.Core.Exceptions;
using FSH.Modules.Accounting.Contracts.v1.Consumption;
using FSH.Modules.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Modules.Accounting.Features.v1.Consumption.GetConsumption;

public record GetConsumptionQuery(Guid Id) : IQuery<ConsumptionDto>;

public class GetConsumptionHandler(AccountingDbContext context) : IQueryHandler<GetConsumptionQuery, ConsumptionDto>
{
    public async ValueTask<ConsumptionDto> Handle(GetConsumptionQuery query, CancellationToken ct)
    {
        var entity = await context.Consumption
            .Where(x => x.Id == query.Id)
            .Select(x => new ConsumptionDto(
                x.Id,
                x.Name,
                x.Description,
                x.IsActive,
                x.CreatedOnUtc))
            .FirstOrDefaultAsync(ct);
        
        return entity ?? throw new NotFoundException("Consumption not found");
    }
}
