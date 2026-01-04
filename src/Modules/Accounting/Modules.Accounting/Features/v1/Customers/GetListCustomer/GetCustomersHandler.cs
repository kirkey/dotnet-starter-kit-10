using FSH.Modules.Accounting.Contracts.v1.Customers;
using FSH.Modules.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Modules.Accounting.Features.v1.Customers.GetCustomers;

public record GetCustomersQuery(
    int Page = 1,
    int PageSize = 10,
    string? SearchTerm = null,
    bool? IsActive = null) : IQuery<CustomersPagedResponse>;

public record CustomersPagedResponse(
    List<CustomerSummaryDto> Items,
    int TotalCount,
    int Page,
    int PageSize);

public class GetCustomersHandler(AccountingDbContext context) 
    : IQueryHandler<GetCustomersQuery, CustomersPagedResponse>
{
    public async ValueTask<CustomersPagedResponse> Handle(GetCustomersQuery query, CancellationToken ct)
    {
        var queryable = context.Customers.AsQueryable();
        
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
            .Select(x => new CustomerSummaryDto(x.Id, x.Name, x.IsActive))
            .ToListAsync(ct);
        
        return new CustomersPagedResponse(items, totalCount, query.Page, query.PageSize);
    }
}
