using FSH.Module.Accounting.Contracts.v1.Checks;
using FSH.Module.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Module.Accounting.Features.v1.Checks.GetChecks;

public record GetChecksQuery(
    int Page = 1,
    int PageSize = 10,
    string? SearchTerm = null,
    bool? IsActive = null,
    string? Status = null,
    DateTime? FromDate = null,
    DateTime? ToDate = null) : IQuery<ChecksPagedResponse>;

public record ChecksPagedResponse(
    List<CheckSummaryDto> Items,
    int TotalCount,
    int Page,
    int PageSize);

public class GetChecksHandler(AccountingDbContext context) 
    : IQueryHandler<GetChecksQuery, ChecksPagedResponse>
{
    public async ValueTask<ChecksPagedResponse> Handle(GetChecksQuery query, CancellationToken ct)
    {
        var queryable = context.Checks.AsQueryable();
        
        if (!string.IsNullOrWhiteSpace(query.SearchTerm))
        {
            queryable = queryable.Where(x => 
                x.CheckNumber.Contains(query.SearchTerm) ||
                x.PayeeName.Contains(query.SearchTerm) ||
                x.AccountNumber.Contains(query.SearchTerm));
        }
        
        if (query.IsActive.HasValue)
        {
            queryable = queryable.Where(x => x.IsActive == query.IsActive.Value);
        }
        
        if (!string.IsNullOrWhiteSpace(query.Status))
        {
            queryable = queryable.Where(x => x.Status == query.Status);
        }
        
        if (query.FromDate.HasValue)
        {
            queryable = queryable.Where(x => x.CheckDate >= query.FromDate.Value);
        }
        
        if (query.ToDate.HasValue)
        {
            queryable = queryable.Where(x => x.CheckDate <= query.ToDate.Value);
        }
        
        var totalCount = await queryable.CountAsync(ct);
        
        var items = await queryable
            .OrderByDescending(x => x.CheckDate)
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .Select(x => new CheckSummaryDto(
                x.Id,
                x.CheckNumber,
                x.CheckDate,
                x.PayeeName,
                x.Amount,
                x.Status,
                x.IsActive))
            .ToListAsync(ct);
        
        return new ChecksPagedResponse(items, totalCount, query.Page, query.PageSize);
    }
}
