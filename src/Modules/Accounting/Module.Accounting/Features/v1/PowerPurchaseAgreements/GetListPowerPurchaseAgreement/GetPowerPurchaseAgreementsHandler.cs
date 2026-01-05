using FSH.Module.Accounting.Contracts.v1.PowerPurchaseAgreements.GetListPowerPurchaseAgreement;
using FSH.Module.Accounting.Contracts.v1.PowerPurchaseAgreements;
using FSH.Module.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Module.Accounting.Features.v1.PowerPurchaseAgreements.GetPowerPurchaseAgreements;

public class GetPowerPurchaseAgreementsHandler(AccountingDbContext context) 
    : IQueryHandler<GetPowerPurchaseAgreementsQuery, PowerPurchaseAgreementsPagedResponse>
{
    public async ValueTask<PowerPurchaseAgreementsPagedResponse> Handle(GetPowerPurchaseAgreementsQuery query, CancellationToken ct)
    {
        var queryable = context.PowerPurchaseAgreements.AsQueryable();
        
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
            .Select(x => new PowerPurchaseAgreementSummaryDto(x.Id, x.Name, x.IsActive))
            .ToListAsync(ct);
        
        return new PowerPurchaseAgreementsPagedResponse(items, totalCount, query.Page, query.PageSize);
    }
}
