using FSH.Modules.Accounting.Contracts.v1.BankReconciliations;
using FSH.Modules.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Modules.Accounting.Features.v1.BankReconciliations.GetBankReconciliations;

public record GetBankReconciliationsQuery(
    int Page = 1,
    int PageSize = 10,
    string? SearchTerm = null,
    bool? IsActive = null,
    Guid? BankAccountId = null,
    string? Status = null,
    DateTime? FromDate = null,
    DateTime? ToDate = null) : IQuery<BankReconciliationsPagedResponse>;

public record BankReconciliationsPagedResponse(
    List<BankReconciliationSummaryDto> Items,
    int TotalCount,
    int Page,
    int PageSize);

public class GetBankReconciliationsHandler(AccountingDbContext context) 
    : IQueryHandler<GetBankReconciliationsQuery, BankReconciliationsPagedResponse>
{
    public async ValueTask<BankReconciliationsPagedResponse> Handle(GetBankReconciliationsQuery query, CancellationToken ct)
    {
        var queryable = context.BankReconciliations.AsQueryable();
        
        if (query.BankAccountId.HasValue)
        {
            queryable = queryable.Where(x => x.BankAccountId == query.BankAccountId.Value);
        }
        
        if (!string.IsNullOrWhiteSpace(query.SearchTerm))
        {
            queryable = queryable.Where(x => x.ReconciliationNumber.Contains(query.SearchTerm));
        }
        
        if (query.IsActive.HasValue)
        {
            queryable = queryable.Where(x => x.IsActive == query.IsActive.Value);
        }
        
        if (!string.IsNullOrWhiteSpace(query.Status))
        {
            queryable = queryable.Where(x => x.Status == query.Status);
        }
        
        if (query.FromDate.HasValue)
        {
            queryable = queryable.Where(x => x.StatementDate >= query.FromDate.Value);
        }
        
        if (query.ToDate.HasValue)
        {
            queryable = queryable.Where(x => x.StatementDate <= query.ToDate.Value);
        }
        
        var totalCount = await queryable.CountAsync(ct);
        
        var items = await queryable
            .OrderByDescending(x => x.StatementDate)
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .Select(x => new BankReconciliationSummaryDto(
                x.Id,
                x.ReconciliationNumber,
                x.BankAccountId,
                x.StatementDate,
                x.StatementBalance,
                x.Difference,
                x.Status,
                x.IsActive))
            .ToListAsync(ct);
        
        return new BankReconciliationsPagedResponse(items, totalCount, query.Page, query.PageSize);
    }
}
