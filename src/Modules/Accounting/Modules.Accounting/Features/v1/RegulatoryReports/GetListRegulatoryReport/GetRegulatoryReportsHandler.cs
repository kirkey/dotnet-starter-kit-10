using FSH.Modules.Accounting.Contracts.v1.RegulatoryReports;
using FSH.Modules.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Modules.Accounting.Features.v1.RegulatoryReports.GetRegulatoryReports;

public record GetRegulatoryReportsQuery(
    int Page = 1,
    int PageSize = 10,
    string? SearchTerm = null,
    bool? IsActive = null) : IQuery<RegulatoryReportsPagedResponse>;

public record RegulatoryReportsPagedResponse(
    List<RegulatoryReportSummaryDto> Items,
    int TotalCount,
    int Page,
    int PageSize);

public class GetRegulatoryReportsHandler(AccountingDbContext context) 
    : IQueryHandler<GetRegulatoryReportsQuery, RegulatoryReportsPagedResponse>
{
    public async ValueTask<RegulatoryReportsPagedResponse> Handle(GetRegulatoryReportsQuery query, CancellationToken ct)
    {
        var queryable = context.RegulatoryReports.AsQueryable();
        
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
            .Select(x => new RegulatoryReportSummaryDto(x.Id, x.Name, x.IsActive))
            .ToListAsync(ct);
        
        return new RegulatoryReportsPagedResponse(items, totalCount, query.Page, query.PageSize);
    }
}
