using FSH.Modules.Microfinance.Contracts.v1.LoanOfficerAssignments;
using FSH.Modules.Microfinance.Data;

namespace FSH.Modules.Microfinance.Features.v1.LoanOfficerAssignments.GetLoanOfficerAssignments;

public record GetLoanOfficerAssignmentsQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive) : IQuery<LoanOfficerAssignmentsPagedResponse>;

public class GetLoanOfficerAssignmentsHandler(MicrofinanceDbContext context) : IQueryHandler<GetLoanOfficerAssignmentsQuery, LoanOfficerAssignmentsPagedResponse>
{
    public async ValueTask<LoanOfficerAssignmentsPagedResponse> Handle(GetLoanOfficerAssignmentsQuery query, CancellationToken ct)
    {
        var queryable = context.LoanOfficerAssignments.AsQueryable();
        
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
            .Select(x => new LoanOfficerAssignmentSummaryDto(x.Id, x.Name, x.IsActive))
            .ToListAsync(ct);
        
        return new LoanOfficerAssignmentsPagedResponse(items, totalCount, query.Page, query.PageSize);
    }
}
