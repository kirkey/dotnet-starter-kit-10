using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.LoanWriteOffs.GetLoanWriteOffs;
using FSH.Module.Microfinance.Contracts.v1.LoanWriteOffs;

namespace FSH.Module.Microfinance.Features.v1.LoanWriteOffs.GetLoanWriteOffs;

public class GetLoanWriteOffsHandler(MicrofinanceDbContext context) : IQueryHandler<GetLoanWriteOffsQuery, LoanWriteOffsPagedResponse>
{
    public async ValueTask<LoanWriteOffsPagedResponse> Handle(GetLoanWriteOffsQuery query, CancellationToken ct)
    {
        var queryable = context.LoanWriteOffs.AsQueryable();
        
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
            .Select(x => new LoanWriteOffSummaryDto(x.Id, x.Name, x.IsActive))
            .ToListAsync(ct);
        
        return new LoanWriteOffsPagedResponse(items, totalCount, query.Page, query.PageSize);
    }
}
