using FSH.Modules.Accounting.Contracts.v1.PaymentAllocations;
using FSH.Modules.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Modules.Accounting.Features.v1.PaymentAllocations.GetPaymentAllocations;

public record GetPaymentAllocationsQuery(
    int Page = 1,
    int PageSize = 10,
    string? SearchTerm = null,
    bool? IsActive = null) : IQuery<PaymentAllocationsPagedResponse>;

public record PaymentAllocationsPagedResponse(
    List<PaymentAllocationSummaryDto> Items,
    int TotalCount,
    int Page,
    int PageSize);

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
