using FSH.Module.Accounting.Contracts.v1.GeneralLedger;
using FSH.Module.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Module.Accounting.Features.v1.GeneralLedger.GetListGeneralLedger;

public record GetGeneralLedgerListQuery(
    int Page = 1,
    int PageSize = 10,
    string? SearchTerm = null,
    bool? IsActive = null) : IQuery<GeneralLedgerPagedResponse>;

public record GeneralLedgerPagedResponse(
    List<GeneralLedgerSummaryDto> Items,
    int TotalCount,
    int Page,
    int PageSize);

public class GetGeneralLedgerListHandler(AccountingDbContext context) 
    : IQueryHandler<GetGeneralLedgerListQuery, GeneralLedgerPagedResponse>
{
    public async ValueTask<GeneralLedgerPagedResponse> Handle(GetGeneralLedgerListQuery query, CancellationToken ct)
    {
        var queryable = context.GeneralLedger.AsQueryable();
        
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
            .Select(x => new GeneralLedgerSummaryDto(x.Id, x.Name, x.IsActive))
            .ToListAsync(ct);
        
        return new GeneralLedgerPagedResponse(items, totalCount, query.Page, query.PageSize);
    }
}
