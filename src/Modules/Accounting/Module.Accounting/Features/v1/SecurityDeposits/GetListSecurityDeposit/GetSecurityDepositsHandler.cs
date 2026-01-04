using FSH.Module.Accounting.Contracts.v1.SecurityDeposits;
using FSH.Module.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Module.Accounting.Features.v1.SecurityDeposits.GetSecurityDeposits;

public record GetSecurityDepositsQuery(
    int Page = 1,
    int PageSize = 10,
    string? SearchTerm = null,
    bool? IsActive = null) : IQuery<SecurityDepositsPagedResponse>;

public record SecurityDepositsPagedResponse(
    List<SecurityDepositSummaryDto> Items,
    int TotalCount,
    int Page,
    int PageSize);

public class GetSecurityDepositsHandler(AccountingDbContext context) 
    : IQueryHandler<GetSecurityDepositsQuery, SecurityDepositsPagedResponse>
{
    public async ValueTask<SecurityDepositsPagedResponse> Handle(GetSecurityDepositsQuery query, CancellationToken ct)
    {
        var queryable = context.SecurityDeposits.AsQueryable();
        
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
            .Select(x => new SecurityDepositSummaryDto(x.Id, x.Name, x.IsActive))
            .ToListAsync(ct);
        
        return new SecurityDepositsPagedResponse(items, totalCount, query.Page, query.PageSize);
    }
}
