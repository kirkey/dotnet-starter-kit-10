using FSH.Framework.Core.Exceptions;
using FSH.Modules.Accounting.Contracts.v1.CostCenters;
using FSH.Modules.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Modules.Accounting.Features.v1.CostCenters.GetCostCenter;

public record GetCostCenterQuery(Guid Id) : IQuery<CostCenterDto>;

public class GetCostCenterHandler(AccountingDbContext context) : IQueryHandler<GetCostCenterQuery, CostCenterDto>
{
    public async ValueTask<CostCenterDto> Handle(GetCostCenterQuery query, CancellationToken ct)
    {
        var entity = await context.CostCenters
            .Where(x => x.Id == query.Id)
            .Select(x => new CostCenterDto(
                x.Id,
                x.Name,
                x.Description,
                x.IsActive,
                x.CreatedOnUtc))
            .FirstOrDefaultAsync(ct);
        
        return entity ?? throw new NotFoundException("CostCenter not found");
    }
}
