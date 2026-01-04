using FSH.Modules.Accounting.Contracts.v1.ChartOfAccounts;
using FSH.Modules.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Modules.Accounting.Features.v1.ChartOfAccounts.GetChartOfAccounts;

public record GetChartOfAccountsQuery(
    int Page = 1,
    int PageSize = 10,
    string? SearchTerm = null,
    bool? IsActive = null,
    string? AccountType = null) : IQuery<ChartOfAccountsPagedResponse>;

public record ChartOfAccountsPagedResponse(
    List<ChartOfAccountSummaryDto> Items,
    int TotalCount,
    int Page,
    int PageSize);

public class GetChartOfAccountsHandler(AccountingDbContext context) 
    : IQueryHandler<GetChartOfAccountsQuery, ChartOfAccountsPagedResponse>
{
    public async ValueTask<ChartOfAccountsPagedResponse> Handle(GetChartOfAccountsQuery query, CancellationToken ct)
    {
        var queryable = context.ChartOfAccounts.AsQueryable();
        
        if (!string.IsNullOrWhiteSpace(query.SearchTerm))
        {
            queryable = queryable.Where(x => 
                x.AccountCode.Contains(query.SearchTerm) || 
                x.AccountName.Contains(query.SearchTerm));
        }
        
        if (query.IsActive.HasValue)
        {
            queryable = queryable.Where(x => x.IsActive == query.IsActive.Value);
        }
        
        if (!string.IsNullOrWhiteSpace(query.AccountType))
        {
            queryable = queryable.Where(x => x.AccountType == query.AccountType);
        }
        
        var totalCount = await queryable.CountAsync(ct);
        
        var items = await queryable
            .OrderBy(x => x.AccountCode)
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .Select(x => new ChartOfAccountSummaryDto(
                x.Id,
                x.AccountCode,
                x.AccountName,
                x.AccountType,
                x.Balance,
                x.IsActive))
            .ToListAsync(ct);
        
        return new ChartOfAccountsPagedResponse(items, totalCount, query.Page, query.PageSize);
    }
}
