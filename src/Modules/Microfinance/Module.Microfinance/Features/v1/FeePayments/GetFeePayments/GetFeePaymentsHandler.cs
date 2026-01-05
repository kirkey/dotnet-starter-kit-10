using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.FeePayments.GetFeePayments;
using FSH.Module.Microfinance.Contracts.v1.FeePayments;

namespace FSH.Module.Microfinance.Features.v1.FeePayments.GetFeePayments;

public class GetFeePaymentsHandler(MicrofinanceDbContext context) : IQueryHandler<GetFeePaymentsQuery, FeePaymentsPagedResponse>
{
    public async ValueTask<FeePaymentsPagedResponse> Handle(GetFeePaymentsQuery query, CancellationToken ct)
    {
        var queryable = context.FeePayments.AsQueryable();
        
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
            .Select(x => new FeePaymentSummaryDto(x.Id, x.Name, x.IsActive))
            .ToListAsync(ct);
        
        return new FeePaymentsPagedResponse(items, totalCount, query.Page, query.PageSize);
    }
}
