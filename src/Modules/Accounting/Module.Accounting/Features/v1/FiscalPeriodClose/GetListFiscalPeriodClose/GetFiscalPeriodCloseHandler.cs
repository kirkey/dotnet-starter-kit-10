using FSH.Module.Accounting.Contracts.v1.FiscalPeriodClose;
using FSH.Module.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Module.Accounting.Features.v1.FiscalPeriodClose.GetFiscalPeriodClose;

/// <summary>
/// Query to retrieve a paginated list of FiscalPeriodClose entries with optional filtering.
/// </summary>
/// <param name="Page">Page number for pagination (1-based)</param>
/// <param name="PageSize">Items per page</param>
/// <param name="SearchTerm">Optional filter by period name or fiscal year</param>
/// <param name="IsActive">Optional active status filter</param>
/// <param name="FiscalYear">Optional filter by fiscal year</param>
/// <param name="FiscalPeriodId">Optional filter by fiscal period identifier</param>
/// <param name="Status">Optional filter by period close status (Draft, Initiated, Completed)</param>
/// <param name="FromDate">Inclusive StartDate filter</param>
/// <param name="ToDate">Inclusive EndDate filter</param>
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

/// <summary>
/// Response for paginated FiscalPeriodClose summaries.
/// </summary>
public record FiscalPeriodClosePagedResponse(
    List<FiscalPeriodCloseSummaryDto> Items,
    int TotalCount,
    int Page,
    int PageSize);

/// <summary>
/// Handler for listing FiscalPeriodClose entries with basic filtering and pagination.
/// </summary>
/// <remarks>
/// Responsibility: Build queryable with optional filters (SearchTerm, Status, FiscalYear, Date range), compute total count, and return paged summary DTOs ordered by StartDate DESC.
/// 
/// Use Cases: Period management dashboard, administrative review prior to initiating or completing close
/// 
/// Permissions: Requires FiscalPeriodClose.Search/View
/// 
/// Exceptions: None; returns empty page when no matches
/// </remarks>
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
