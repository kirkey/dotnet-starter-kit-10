using FSH.Module.Microfinance.Contracts.v1.SavingsTransactions;
using FSH.Module.Microfinance.Data;

namespace FSH.Module.Microfinance.Features.v1.SavingsTransactions.GetSavingsTransactions;

public record GetSavingsTransactionsQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive) : IQuery<SavingsTransactionsPagedResponse>;

public class GetSavingsTransactionsHandler(MicrofinanceDbContext context) : IQueryHandler<GetSavingsTransactionsQuery, SavingsTransactionsPagedResponse>
{
    public async ValueTask<SavingsTransactionsPagedResponse> Handle(GetSavingsTransactionsQuery query, CancellationToken ct)
    {
        var queryable = context.SavingsTransactions.AsQueryable();
        
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
            .Select(x => new SavingsTransactionSummaryDto(x.Id, x.Name, x.IsActive))
            .ToListAsync(ct);
        
        return new SavingsTransactionsPagedResponse(items, totalCount, query.Page, query.PageSize);
    }
}
