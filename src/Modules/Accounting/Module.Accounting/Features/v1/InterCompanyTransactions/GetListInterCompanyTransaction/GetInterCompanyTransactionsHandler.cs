using FSH.Module.Accounting.Contracts.v1.InterCompanyTransactions.GetListInterCompanyTransaction;
using FSH.Module.Accounting.Contracts.v1.InterCompanyTransactions;
using FSH.Module.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Module.Accounting.Features.v1.InterCompanyTransactions.GetInterCompanyTransactions;

public class GetInterCompanyTransactionsHandler(AccountingDbContext context) 
    : IQueryHandler<GetInterCompanyTransactionsQuery, InterCompanyTransactionsPagedResponse>
{
    public async ValueTask<InterCompanyTransactionsPagedResponse> Handle(GetInterCompanyTransactionsQuery query, CancellationToken ct)
    {
        var queryable = context.InterCompanyTransactions.AsQueryable();
        
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
            .Select(x => new InterCompanyTransactionSummaryDto(x.Id, x.Name, x.IsActive))
            .ToListAsync(ct);
        
        return new InterCompanyTransactionsPagedResponse(items, totalCount, query.Page, query.PageSize);
    }
}
