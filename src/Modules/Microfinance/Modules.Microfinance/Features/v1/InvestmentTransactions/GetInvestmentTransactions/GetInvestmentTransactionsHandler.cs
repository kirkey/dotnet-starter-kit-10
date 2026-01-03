using FSH.Modules.Microfinance.Contracts.v1.InvestmentTransactions;
using FSH.Modules.Microfinance.Data;

namespace FSH.Modules.Microfinance.Features.v1.InvestmentTransactions.GetInvestmentTransactions;

public record GetInvestmentTransactionsQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive) : IQuery<InvestmentTransactionsPagedResponse>;

public class GetInvestmentTransactionsHandler(MicrofinanceDbContext context) : IQueryHandler<GetInvestmentTransactionsQuery, InvestmentTransactionsPagedResponse>
{
    public async ValueTask<InvestmentTransactionsPagedResponse> Handle(GetInvestmentTransactionsQuery query, CancellationToken ct)
    {
        var queryable = context.InvestmentTransactions.AsQueryable();
        
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
            .Select(x => new InvestmentTransactionSummaryDto(x.Id, x.Name, x.IsActive))
            .ToListAsync(ct);
        
        return new InvestmentTransactionsPagedResponse(items, totalCount, query.Page, query.PageSize);
    }
}
