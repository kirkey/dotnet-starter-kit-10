using FSH.Modules.Accounting.Contracts.v1.AccountsReceivable;
using FSH.Modules.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Modules.Accounting.Features.v1.AccountsReceivable.GetAccountsReceivable;

public record GetAccountsReceivableQuery(
    int Page = 1,
    int PageSize = 10,
    string? SearchTerm = null,
    bool? IsActive = null) : IQuery<AccountsReceivablePagedResponse>;

public record AccountsReceivablePagedResponse(
    List<AccountsReceivableAccountSummaryDto> Items,
    int TotalCount,
    int Page,
    int PageSize);

public class GetAccountsReceivableHandler(AccountingDbContext context) 
    : IQueryHandler<GetAccountsReceivableQuery, AccountsReceivablePagedResponse>
{
    public async ValueTask<AccountsReceivablePagedResponse> Handle(GetAccountsReceivableQuery query, CancellationToken ct)
    {
        var queryable = context.AccountsReceivable.AsQueryable();
        
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
            .Select(x => new AccountsReceivableAccountSummaryDto(x.Id, x.Name, x.IsActive))
            .ToListAsync(ct);
        
        return new AccountsReceivablePagedResponse(items, totalCount, query.Page, query.PageSize);
    }
}
