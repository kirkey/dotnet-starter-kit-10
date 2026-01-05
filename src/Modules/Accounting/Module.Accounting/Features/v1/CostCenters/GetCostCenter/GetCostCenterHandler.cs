using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Contracts.v1.CostCenters;
using FSH.Module.Accounting.Contracts.v1.CostCenters.GetCostCenter;
using FSH.Module.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Module.Accounting.Features.v1.CostCenters.GetCostCenter;

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
