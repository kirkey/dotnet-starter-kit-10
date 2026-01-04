using FSH.Module.Microfinance.Contracts.v1.CreditBureauReports;
using FSH.Module.Microfinance.Data;

namespace FSH.Module.Microfinance.Features.v1.CreditBureauReports.GetCreditBureauReports;

public record GetCreditBureauReportsQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive) : IQuery<CreditBureauReportsPagedResponse>;

public class GetCreditBureauReportsHandler(MicrofinanceDbContext context) : IQueryHandler<GetCreditBureauReportsQuery, CreditBureauReportsPagedResponse>
{
    public async ValueTask<CreditBureauReportsPagedResponse> Handle(GetCreditBureauReportsQuery query, CancellationToken ct)
    {
        var queryable = context.CreditBureauReports.AsQueryable();
        
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
            .Select(x => new CreditBureauReportSummaryDto(x.Id, x.Name, x.IsActive))
            .ToListAsync(ct);
        
        return new CreditBureauReportsPagedResponse(items, totalCount, query.Page, query.PageSize);
    }
}
