using FSH.Module.Microfinance.Contracts.v1.QrPayments;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.QrPayments.GetQrPayments;

namespace FSH.Module.Microfinance.Features.v1.QrPayments.GetQrPayments;

public class GetQrPaymentsHandler(MicrofinanceDbContext context) : IQueryHandler<GetQrPaymentsQuery, QrPaymentsPagedResponse>
{
    public async ValueTask<QrPaymentsPagedResponse> Handle(GetQrPaymentsQuery query, CancellationToken ct)
    {
        var queryable = context.QrPayments.AsQueryable();
        
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
            .Select(x => new QrPaymentSummaryDto(x.Id, x.Name, x.IsActive))
            .ToListAsync(ct);
        
        return new QrPaymentsPagedResponse(items, totalCount, query.Page, query.PageSize);
    }
}
