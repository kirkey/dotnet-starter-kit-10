using FSH.Module.Microfinance.Contracts.v1.CommunicationTemplates;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.CommunicationTemplates.GetCommunicationTemplates;

namespace FSH.Module.Microfinance.Features.v1.CommunicationTemplates.GetCommunicationTemplates;

public class GetCommunicationTemplatesHandler(MicrofinanceDbContext context) : IQueryHandler<GetCommunicationTemplatesQuery, CommunicationTemplatesPagedResponse>
{
    public async ValueTask<CommunicationTemplatesPagedResponse> Handle(GetCommunicationTemplatesQuery query, CancellationToken ct)
    {
        var queryable = context.CommunicationTemplates.AsQueryable();
        
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
            .Select(x => new CommunicationTemplateSummaryDto(x.Id, x.Name, x.IsActive))
            .ToListAsync(ct);
        
        return new CommunicationTemplatesPagedResponse(items, totalCount, query.Page, query.PageSize);
    }
}
