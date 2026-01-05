using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.ReportDefinitions.GetReportDefinitions;
using FSH.Module.Microfinance.Contracts.v1.ReportDefinitions;

namespace FSH.Module.Microfinance.Features.v1.ReportDefinitions.GetReportDefinitions;

public class GetReportDefinitionsHandler(MicrofinanceDbContext context) : IQueryHandler<GetReportDefinitionsQuery, ReportDefinitionsPagedResponse>
{
    public async ValueTask<ReportDefinitionsPagedResponse> Handle(GetReportDefinitionsQuery query, CancellationToken ct)
    {
        var queryable = context.ReportDefinitions.AsQueryable();
        
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
            .Select(x => new ReportDefinitionSummaryDto(x.Id, x.Name, x.IsActive))
            .ToListAsync(ct);
        
        return new ReportDefinitionsPagedResponse(items, totalCount, query.Page, query.PageSize);
    }
}
