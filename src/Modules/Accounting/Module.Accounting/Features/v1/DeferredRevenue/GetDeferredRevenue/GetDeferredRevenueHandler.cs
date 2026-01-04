using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Contracts.v1.DeferredRevenue;
using FSH.Module.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Module.Accounting.Features.v1.DeferredRevenue.GetDeferredRevenue;

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
