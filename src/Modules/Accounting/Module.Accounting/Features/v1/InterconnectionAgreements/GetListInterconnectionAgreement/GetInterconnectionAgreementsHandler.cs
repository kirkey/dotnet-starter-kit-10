using FSH.Module.Accounting.Contracts.v1.InterconnectionAgreements;
using FSH.Module.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

using FSH.Module.Accounting.Contracts.v1.InterconnectionAgreements.GetListInterconnectionAgreement;

namespace FSH.Module.Accounting.Features.v1.InterconnectionAgreements.GetInterconnectionAgreements;

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
