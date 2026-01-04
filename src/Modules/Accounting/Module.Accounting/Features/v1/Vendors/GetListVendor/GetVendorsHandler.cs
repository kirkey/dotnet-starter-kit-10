using FSH.Module.Accounting.Contracts.v1.Vendors;
using FSH.Module.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Module.Accounting.Features.v1.Vendors.GetVendors;

public record GetVendorsQuery(
    int Page = 1,
    int PageSize = 10,
    string? SearchTerm = null,
    bool? IsActive = null) : IQuery<VendorsPagedResponse>;

public record VendorsPagedResponse(
    List<VendorSummaryDto> Items,
    int TotalCount,
    int Page,
    int PageSize);

public class GetVendorsHandler(AccountingDbContext context) 
    : IQueryHandler<GetVendorsQuery, VendorsPagedResponse>
{
    public async ValueTask<VendorsPagedResponse> Handle(GetVendorsQuery query, CancellationToken ct)
    {
        var queryable = context.Vendors.AsQueryable();
        
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
            .Select(x => new VendorSummaryDto(x.Id, x.Name, x.IsActive))
            .ToListAsync(ct);
        
        return new VendorsPagedResponse(items, totalCount, query.Page, query.PageSize);
    }
}
