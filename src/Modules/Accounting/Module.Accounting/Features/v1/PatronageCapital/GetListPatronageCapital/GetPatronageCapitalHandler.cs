using FSH.Module.Accounting.Contracts.v1.PatronageCapital;
using FSH.Module.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Module.Accounting.Features.v1.PatronageCapital.GetListPatronageCapital;

public record GetPatronageCapitalQuery(
    int Page = 1,
    int PageSize = 10,
    string? SearchTerm = null,
    bool? IsActive = null) : IQuery<PatronageCapitalPagedResponse>;

public record PatronageCapitalPagedResponse(
    List<PatronageCapitalSummaryDto> Items,
    int TotalCount,
    int Page,
    int PageSize);

public class GetPatronageCapitalHandler(AccountingDbContext context) 
    : IQueryHandler<GetPatronageCapitalQuery, PatronageCapitalPagedResponse>
{
    public async ValueTask<PatronageCapitalPagedResponse> Handle(GetPatronageCapitalQuery query, CancellationToken ct)
    {
        var queryable = context.PatronageCapital.AsQueryable();
        
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
            .Select(x => new PatronageCapitalSummaryDto(x.Id, x.Name, x.IsActive))
            .ToListAsync(ct);
        
        return new PatronageCapitalPagedResponse(items, totalCount, query.Page, query.PageSize);
    }
}
