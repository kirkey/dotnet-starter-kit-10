using FSH.Module.Accounting.Contracts.v1.AccountsReceivable;
using FSH.Module.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

using FSH.Module.Accounting.Contracts.v1.AccountsReceivable.GetListAccountsReceivableAccount;

namespace FSH.Module.Accounting.Features.v1.AccountsReceivable.GetAccountsReceivable;

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
