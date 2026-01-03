using FSH.Modules.Microfinance.Contracts.v1.LoanOfficerTargets;
using FSH.Modules.Microfinance.Data;

namespace FSH.Modules.Microfinance.Features.v1.LoanOfficerTargets.GetLoanOfficerTargets;

public record GetLoanOfficerTargetsQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive) : IQuery<LoanOfficerTargetsPagedResponse>;

public class GetLoanOfficerTargetsHandler(MicrofinanceDbContext context) : IQueryHandler<GetLoanOfficerTargetsQuery, LoanOfficerTargetsPagedResponse>
{
    public async ValueTask<LoanOfficerTargetsPagedResponse> Handle(GetLoanOfficerTargetsQuery query, CancellationToken ct)
    {
        var queryable = context.LoanOfficerTargets.AsQueryable();
        
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
            .Select(x => new LoanOfficerTargetSummaryDto(x.Id, x.Name, x.IsActive))
            .ToListAsync(ct);
        
        return new LoanOfficerTargetsPagedResponse(items, totalCount, query.Page, query.PageSize);
    }
}
