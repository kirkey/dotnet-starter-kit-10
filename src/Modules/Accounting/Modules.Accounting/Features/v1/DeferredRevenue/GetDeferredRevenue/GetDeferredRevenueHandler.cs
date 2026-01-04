using FSH.Framework.Core.Exceptions;
using FSH.Modules.Accounting.Contracts.v1.DeferredRevenue;
using FSH.Modules.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Modules.Accounting.Features.v1.DeferredRevenue.GetDeferredRevenue;

public record GetDeferredRevenueQuery(Guid Id) : IQuery<DeferredRevenueDto>;

public class GetDeferredRevenueHandler(AccountingDbContext context) : IQueryHandler<GetDeferredRevenueQuery, DeferredRevenueDto>
{
    public async ValueTask<DeferredRevenueDto> Handle(GetDeferredRevenueQuery query, CancellationToken ct)
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
