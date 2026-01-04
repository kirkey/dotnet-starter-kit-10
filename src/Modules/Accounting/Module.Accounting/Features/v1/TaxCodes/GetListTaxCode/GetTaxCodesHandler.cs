using FSH.Module.Accounting.Contracts.v1.TaxCodes;
using FSH.Module.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Module.Accounting.Features.v1.TaxCodes.GetTaxCodes;

public record GetTaxCodesQuery(
    int Page = 1,
    int PageSize = 10,
    string? SearchTerm = null,
    bool? IsActive = null) : IQuery<TaxCodesPagedResponse>;

public record TaxCodesPagedResponse(
    List<TaxCodeSummaryDto> Items,
    int TotalCount,
    int Page,
    int PageSize);

public class GetTaxCodesHandler(AccountingDbContext context) 
    : IQueryHandler<GetTaxCodesQuery, TaxCodesPagedResponse>
{
    public async ValueTask<TaxCodesPagedResponse> Handle(GetTaxCodesQuery query, CancellationToken ct)
    {
        var queryable = context.TaxCodes.AsQueryable();
        
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
            .Select(x => new TaxCodeSummaryDto(x.Id, x.Name, x.IsActive))
            .ToListAsync(ct);
        
        return new TaxCodesPagedResponse(items, totalCount, query.Page, query.PageSize);
    }
}
