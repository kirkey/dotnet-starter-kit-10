using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Contracts.v1.Consumption;
using FSH.Module.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;
using FSH.Module.Accounting.Contracts.v1.Consumption.GetConsumption;

namespace FSH.Module.Accounting.Features.v1.Consumption.GetConsumption;

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
