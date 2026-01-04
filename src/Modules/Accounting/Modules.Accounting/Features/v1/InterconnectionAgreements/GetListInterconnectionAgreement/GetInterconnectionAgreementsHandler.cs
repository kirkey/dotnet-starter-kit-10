using FSH.Modules.Accounting.Contracts.v1.InterconnectionAgreements;
using FSH.Modules.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Modules.Accounting.Features.v1.InterconnectionAgreements.GetInterconnectionAgreements;

public record GetInterconnectionAgreementsQuery(
    int Page = 1,
    int PageSize = 10,
    string? SearchTerm = null,
    bool? IsActive = null) : IQuery<InterconnectionAgreementsPagedResponse>;

public record InterconnectionAgreementsPagedResponse(
    List<InterconnectionAgreementSummaryDto> Items,
    int TotalCount,
    int Page,
    int PageSize);

public class GetInterconnectionAgreementsHandler(AccountingDbContext context) 
    : IQueryHandler<GetInterconnectionAgreementsQuery, InterconnectionAgreementsPagedResponse>
{
    public async ValueTask<InterconnectionAgreementsPagedResponse> Handle(GetInterconnectionAgreementsQuery query, CancellationToken ct)
    {
        var queryable = context.InterconnectionAgreements.AsQueryable();
        
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
            .Select(x => new InterconnectionAgreementSummaryDto(x.Id, x.Name, x.IsActive))
            .ToListAsync(ct);
        
        return new InterconnectionAgreementsPagedResponse(items, totalCount, query.Page, query.PageSize);
    }
}
