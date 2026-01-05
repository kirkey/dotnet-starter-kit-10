using FSH.Module.Accounting.Contracts.v1.AccountsPayable;
using FSH.Module.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;
using FSH.Module.Accounting.Contracts.v1.AccountsPayable.GetListAccountsPayableAccount;

namespace FSH.Module.Accounting.Features.v1.AccountsPayable.GetAccountsPayable;

public class GetAccountsPayableHandler(AccountingDbContext context) 
    : IQueryHandler<GetAccountsPayableQuery, AccountsPayablePagedResponse>
{
    public async ValueTask<AccountsPayablePagedResponse> Handle(GetAccountsPayableQuery query, CancellationToken ct)
    {
        var queryable = context.AccountsPayable.AsQueryable();
        
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
            .Select(x => new AccountsPayableAccountSummaryDto(x.Id, x.Name, x.IsActive))
            .ToListAsync(ct);
        
        return new AccountsPayablePagedResponse(items, totalCount, query.Page, query.PageSize);
    }
}
