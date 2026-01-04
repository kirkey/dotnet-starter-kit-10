using FSH.Modules.Accounting.Contracts.v1.Banks;
using FSH.Modules.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Modules.Accounting.Features.v1.Banks.GetBanks;

public record GetBanksQuery(
    int Page = 1,
    int PageSize = 10,
    string? SearchTerm = null,
    bool? IsActive = null,
    string? CurrencyCode = null,
    bool? IsDefault = null) : IQuery<BanksPagedResponse>;

public record BanksPagedResponse(
    List<BankSummaryDto> Items,
    int TotalCount,
    int Page,
    int PageSize);

public class GetBanksHandler(AccountingDbContext context) 
    : IQueryHandler<GetBanksQuery, BanksPagedResponse>
{
    public async ValueTask<BanksPagedResponse> Handle(GetBanksQuery query, CancellationToken ct)
    {
        var queryable = context.Banks.AsQueryable();
        
        if (!string.IsNullOrWhiteSpace(query.SearchTerm))
        {
            queryable = queryable.Where(x => x.BankName.Contains(query.SearchTerm));
        }
        
        if (query.IsActive.HasValue)
        {
            queryable = queryable.Where(x => x.IsActive == query.IsActive.Value);
        }
        
        if (!string.IsNullOrWhiteSpace(query.CurrencyCode))
        {
            queryable = queryable.Where(x => x.CurrencyCode == query.CurrencyCode);
        }
        
        if (query.IsDefault.HasValue)
        {
            queryable = queryable.Where(x => x.IsDefault == query.IsDefault.Value);
        }
        
        var totalCount = await queryable.CountAsync(ct);
        
        var items = await queryable
            .OrderByDescending(x => x.BankName)
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .Select(x => new BankSummaryDto(
                x.Id,
                x.BankName,
                x.CurrencyCode,
                x.CurrentBalance,
                x.IsDefault,
                x.IsActive))
            .ToListAsync(ct);
        
        return new BanksPagedResponse(items, totalCount, query.Page, query.PageSize);
    }
}
