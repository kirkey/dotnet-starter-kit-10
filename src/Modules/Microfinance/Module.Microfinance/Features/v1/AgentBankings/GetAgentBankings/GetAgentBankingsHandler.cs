using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.AgentBankings.GetAgentBankings;
using FSH.Module.Microfinance.Contracts.v1.AgentBankings;

namespace FSH.Module.Microfinance.Features.v1.AgentBankings.GetAgentBankings;

public class GetAgentBankingsHandler(MicrofinanceDbContext context) : IQueryHandler<GetAgentBankingsQuery, AgentBankingsPagedResponse>
{
    public async ValueTask<AgentBankingsPagedResponse> Handle(GetAgentBankingsQuery query, CancellationToken ct)
    {
        var queryable = context.AgentBankings.AsQueryable();
        
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
            .Select(x => new AgentBankingSummaryDto(x.Id, x.Name, x.IsActive))
            .ToListAsync(ct);
        
        return new AgentBankingsPagedResponse(items, totalCount, query.Page, query.PageSize);
    }
}
