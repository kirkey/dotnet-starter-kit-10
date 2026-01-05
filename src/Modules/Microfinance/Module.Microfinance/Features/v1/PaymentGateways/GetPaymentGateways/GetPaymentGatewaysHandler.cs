using FSH.Module.Microfinance.Contracts.v1.PaymentGateways;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.PaymentGateways.GetPaymentGateways;

namespace FSH.Module.Microfinance.Features.v1.PaymentGateways.GetPaymentGateways;

public class GetPaymentGatewaysHandler(MicrofinanceDbContext context) : IQueryHandler<GetPaymentGatewaysQuery, PaymentGatewaysPagedResponse>
{
    public async ValueTask<PaymentGatewaysPagedResponse> Handle(GetPaymentGatewaysQuery query, CancellationToken ct)
    {
        var queryable = context.PaymentGateways.AsQueryable();
        
        if (!string.IsNullOrWhiteSpace(query.SearchTerm))
        {
            queryable = queryable.Where(x => x.Name.Contains(query.SearchTerm));
        }
        
        if (query.IsActive.HasValue)
        {
            queryable = queryable.Where(x => x.IsActive == query.IsActive.Value);
        }
        
        var totalCount = await queryable.CountAsync(ct);
        
        var items = await queryable
            .OrderByDescending(x => x.CreatedOnUtc)
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .Select(x => new PaymentGatewaySummaryDto(x.Id, x.Name, x.IsActive))
            .ToListAsync(ct);
        
        return new PaymentGatewaysPagedResponse(items, totalCount, query.Page, query.PageSize);
    }
}
