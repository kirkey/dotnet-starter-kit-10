using FSH.Modules.Accounting.Contracts.v1.Payments;
using FSH.Modules.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Modules.Accounting.Features.v1.Payments.GetPayments;

public record GetPaymentsQuery(
    int Page = 1,
    int PageSize = 10,
    string? SearchTerm = null,
    bool? IsActive = null) : IQuery<PaymentsPagedResponse>;

public record PaymentsPagedResponse(
    List<PaymentSummaryDto> Items,
    int TotalCount,
    int Page,
    int PageSize);

public class GetPaymentsHandler(AccountingDbContext context) 
    : IQueryHandler<GetPaymentsQuery, PaymentsPagedResponse>
{
    public async ValueTask<PaymentsPagedResponse> Handle(GetPaymentsQuery query, CancellationToken ct)
    {
        var queryable = context.Payments.AsQueryable();
        
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
            .Select(x => new PaymentSummaryDto(x.Id, x.Name, x.IsActive))
            .ToListAsync(ct);
        
        return new PaymentsPagedResponse(items, totalCount, query.Page, query.PageSize);
    }
}
