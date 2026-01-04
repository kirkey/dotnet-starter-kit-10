using FSH.Module.Microfinance.Contracts.v1.ReportGenerations;
using FSH.Module.Microfinance.Data;

namespace FSH.Module.Microfinance.Features.v1.ReportGenerations.GetReportGenerations;

public record GetReportGenerationsQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive) : IQuery<ReportGenerationsPagedResponse>;

public class GetReportGenerationsHandler(MicrofinanceDbContext context) : IQueryHandler<GetReportGenerationsQuery, ReportGenerationsPagedResponse>
{
    public async ValueTask<ReportGenerationsPagedResponse> Handle(GetReportGenerationsQuery query, CancellationToken ct)
    {
        var queryable = context.ReportGenerations.AsQueryable();
        
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
            .Select(x => new ReportGenerationSummaryDto(x.Id, x.Name, x.IsActive))
            .ToListAsync(ct);
        
        return new ReportGenerationsPagedResponse(items, totalCount, query.Page, query.PageSize);
    }
}
