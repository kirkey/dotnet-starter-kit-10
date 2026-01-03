using FSH.Modules.Microfinance.Contracts.v1.CreditBureauInquirys;
using FSH.Modules.Microfinance.Data;

namespace FSH.Modules.Microfinance.Features.v1.CreditBureauInquirys.GetCreditBureauInquirys;

public record GetCreditBureauInquirysQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive) : IQuery<CreditBureauInquirysPagedResponse>;

public class GetCreditBureauInquirysHandler(MicrofinanceDbContext context) : IQueryHandler<GetCreditBureauInquirysQuery, CreditBureauInquirysPagedResponse>
{
    public async ValueTask<CreditBureauInquirysPagedResponse> Handle(GetCreditBureauInquirysQuery query, CancellationToken ct)
    {
        var queryable = context.CreditBureauInquirys.AsQueryable();
        
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
            .Select(x => new CreditBureauInquirySummaryDto(x.Id, x.Name, x.IsActive))
            .ToListAsync(ct);
        
        return new CreditBureauInquirysPagedResponse(items, totalCount, query.Page, query.PageSize);
    }
}
