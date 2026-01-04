using FSH.Module.Microfinance.Contracts.v1.CreditScores;
using FSH.Module.Microfinance.Data;

namespace FSH.Module.Microfinance.Features.v1.CreditScores.GetCreditScores;

public record GetCreditScoresQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive) : IQuery<CreditScoresPagedResponse>;

public class GetCreditScoresHandler(MicrofinanceDbContext context) : IQueryHandler<GetCreditScoresQuery, CreditScoresPagedResponse>
{
    public async ValueTask<CreditScoresPagedResponse> Handle(GetCreditScoresQuery query, CancellationToken ct)
    {
        var queryable = context.CreditScores.AsQueryable();
        
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
            .Select(x => new CreditScoreSummaryDto(x.Id, x.Name, x.IsActive))
            .ToListAsync(ct);
        
        return new CreditScoresPagedResponse(items, totalCount, query.Page, query.PageSize);
    }
}
