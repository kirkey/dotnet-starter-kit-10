using FSH.Module.Accounting.Contracts.v1.PaymentAllocations.GetListPaymentAllocation;
using FSH.Module.Accounting.Contracts.v1.PaymentAllocations;
using FSH.Module.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Module.Accounting.Features.v1.PaymentAllocations.GetPaymentAllocations;

public class GetPaymentAllocationsHandler(AccountingDbContext context) 
    : IQueryHandler<GetPaymentAllocationsQuery, PaymentAllocationsPagedResponse>
{
    public async ValueTask<PaymentAllocationsPagedResponse> Handle(GetPaymentAllocationsQuery query, CancellationToken ct)
    {
        var queryable = context.PaymentAllocations.AsQueryable();
        
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
            .Select(x => new PaymentAllocationSummaryDto(x.Id, x.Name, x.IsActive))
            .ToListAsync(ct);
        
        return new PaymentAllocationsPagedResponse(items, totalCount, query.Page, query.PageSize);
    }
}
