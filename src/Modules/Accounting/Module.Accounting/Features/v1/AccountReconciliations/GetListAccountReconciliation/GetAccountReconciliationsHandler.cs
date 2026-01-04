using FSH.Module.Accounting.Contracts.v1.AccountReconciliations;
using FSH.Module.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Module.Accounting.Features.v1.AccountReconciliations.GetAccountReconciliations;

public record GetAccountReconciliationsQuery(
    int Page = 1,
    int PageSize = 10,
    string? SearchTerm = null,
    bool? IsActive = null) : IQuery<AccountReconciliationsPagedResponse>;

public record AccountReconciliationsPagedResponse(
    List<AccountReconciliationSummaryDto> Items,
    int TotalCount,
    int Page,
    int PageSize);

public class GetAccountReconciliationsHandler(AccountingDbContext context) 
    : IQueryHandler<GetAccountReconciliationsQuery, AccountReconciliationsPagedResponse>
{
    public async ValueTask<AccountReconciliationsPagedResponse> Handle(GetAccountReconciliationsQuery query, CancellationToken ct)
    {
        var queryable = context.AccountReconciliations.AsQueryable();
        
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
            .Select(x => new AccountReconciliationSummaryDto(x.Id, x.Name, x.IsActive))
            .ToListAsync(ct);
        
        return new AccountReconciliationsPagedResponse(items, totalCount, query.Page, query.PageSize);
    }
}
