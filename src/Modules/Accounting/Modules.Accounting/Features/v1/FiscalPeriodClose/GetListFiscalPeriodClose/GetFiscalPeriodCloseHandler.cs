using FSH.Modules.Accounting.Contracts.v1.FiscalPeriodClose;
using FSH.Modules.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Modules.Accounting.Features.v1.FiscalPeriodClose.GetFiscalPeriodClose;

public record GetFiscalPeriodCloseQuery(
    int Page = 1,
    int PageSize = 10,
    string? SearchTerm = null,
    bool? IsActive = null,
    int? FiscalYear = null,
    Guid? FiscalPeriodId = null,
    string? Status = null,
    DateTime? FromDate = null,
    DateTime? ToDate = null) : IQuery<FiscalPeriodClosePagedResponse>;

public record FiscalPeriodClosePagedResponse(
    List<FiscalPeriodCloseSummaryDto> Items,
    int TotalCount,
    int Page,
    int PageSize);

public class GetFiscalPeriodCloseHandler(AccountingDbContext context) 
    : IQueryHandler<GetFiscalPeriodCloseQuery, FiscalPeriodClosePagedResponse>
{
    public async ValueTask<FiscalPeriodClosePagedResponse> Handle(GetFiscalPeriodCloseQuery query, CancellationToken ct)
    {
        var queryable = context.FiscalPeriodClose.AsQueryable();
        
        if (query.FiscalPeriodId.HasValue)
        {
            queryable = queryable.Where(x => x.FiscalPeriodId == query.FiscalPeriodId.Value);
        }
        
        if (!string.IsNullOrWhiteSpace(query.SearchTerm))
        {
            queryable = queryable.Where(x => x.PeriodName.Contains(query.SearchTerm));
        }
        
        if (query.IsActive.HasValue)
        {
            queryable = queryable.Where(x => x.IsActive == query.IsActive.Value);
        }
        
        if (query.FiscalYear.HasValue)
        {
            queryable = queryable.Where(x => x.FiscalYear == query.FiscalYear.Value);
        }
        
        if (!string.IsNullOrWhiteSpace(query.Status))
        {
            queryable = queryable.Where(x => x.Status == query.Status);
        }
        
        if (query.FromDate.HasValue)
        {
            queryable = queryable.Where(x => x.StartDate >= query.FromDate.Value);
        }
        
        if (query.ToDate.HasValue)
        {
            queryable = queryable.Where(x => x.EndDate <= query.ToDate.Value);
        }
        
        var totalCount = await queryable.CountAsync(ct);
        
        var items = await queryable
            .OrderByDescending(x => x.StartDate)
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .Select(x => new FiscalPeriodCloseSummaryDto(
                x.Id,
                x.FiscalYear,
                x.PeriodName,
                x.StartDate,
                x.EndDate,
                x.Status,
                x.IsActive))
            .ToListAsync(ct);
        
        return new FiscalPeriodClosePagedResponse(items, totalCount, query.Page, query.PageSize);
    }
}
