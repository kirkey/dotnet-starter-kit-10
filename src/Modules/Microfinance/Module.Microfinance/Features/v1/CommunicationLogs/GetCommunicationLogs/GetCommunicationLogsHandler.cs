using FSH.Module.Microfinance.Contracts.v1.CommunicationLogs;
using FSH.Module.Microfinance.Data;

namespace FSH.Module.Microfinance.Features.v1.CommunicationLogs.GetCommunicationLogs;

public record GetCommunicationLogsQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive) : IQuery<CommunicationLogsPagedResponse>;

public class GetCommunicationLogsHandler(MicrofinanceDbContext context) : IQueryHandler<GetCommunicationLogsQuery, CommunicationLogsPagedResponse>
{
    public async ValueTask<CommunicationLogsPagedResponse> Handle(GetCommunicationLogsQuery query, CancellationToken ct)
    {
        var queryable = context.CommunicationLogs.AsQueryable();
        
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
            .Select(x => new CommunicationLogSummaryDto(x.Id, x.Name, x.IsActive))
            .ToListAsync(ct);
        
        return new CommunicationLogsPagedResponse(items, totalCount, query.Page, query.PageSize);
    }
}
