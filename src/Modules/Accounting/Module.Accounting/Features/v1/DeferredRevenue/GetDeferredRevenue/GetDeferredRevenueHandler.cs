using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Contracts.v1.DeferredRevenue;
using FSH.Module.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

using FSH.Module.Accounting.Contracts.v1.DeferredRevenue.GetDeferredRevenue;

namespace FSH.Module.Accounting.Features.v1.DeferredRevenue.GetDeferredRevenue;

public class GetDeferredRevenueByIdHandler(AccountingDbContext context) : IQueryHandler<GetDeferredRevenueByIdQuery, DeferredRevenueDto>
{
    public async ValueTask<DeferredRevenueDto> Handle(GetDeferredRevenueByIdQuery query, CancellationToken ct)
    {
        var entity = await context.DeferredRevenue
            .Where(x => x.Id == query.Id)
            .Select(x => new DeferredRevenueDto(
                x.Id,
                x.Name,
                x.Description,
                x.IsActive,
                x.CreatedOnUtc))
            .FirstOrDefaultAsync(ct);
        
        return entity ?? throw new NotFoundException("DeferredRevenue not found");
    }
}
