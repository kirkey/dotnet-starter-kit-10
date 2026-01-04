using FSH.Modules.Accounting.Contracts.v1.JournalEntries;
using FSH.Modules.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Modules.Accounting.Features.v1.JournalEntries.GetJournalEntries;

public record GetJournalEntriesQuery(
    int Page = 1,
    int PageSize = 10,
    string? SearchTerm = null,
    bool? IsActive = null,
    string? Status = null,
    string? EntryType = null,
    DateTime? FromDate = null,
    DateTime? ToDate = null) : IQuery<JournalEntriesPagedResponse>;

public record JournalEntriesPagedResponse(
    List<JournalEntrySummaryDto> Items,
    int TotalCount,
    int Page,
    int PageSize);

public class GetJournalEntriesHandler(AccountingDbContext context) 
    : IQueryHandler<GetJournalEntriesQuery, JournalEntriesPagedResponse>
{
    public async ValueTask<JournalEntriesPagedResponse> Handle(GetJournalEntriesQuery query, CancellationToken ct)
    {
        var queryable = context.JournalEntries.AsQueryable();
        
        if (!string.IsNullOrWhiteSpace(query.SearchTerm))
        {
            queryable = queryable.Where(x => 
                x.EntryNumber.Contains(query.SearchTerm) ||
                x.ReferenceNumber.Contains(query.SearchTerm));
        }
        
        if (query.IsActive.HasValue)
        {
            queryable = queryable.Where(x => x.IsActive == query.IsActive.Value);
        }
        
        if (!string.IsNullOrWhiteSpace(query.Status))
        {
            queryable = queryable.Where(x => x.Status == query.Status);
        }
        
        if (!string.IsNullOrWhiteSpace(query.EntryType))
        {
            queryable = queryable.Where(x => x.EntryType == query.EntryType);
        }
        
        if (query.FromDate.HasValue)
        {
            queryable = queryable.Where(x => x.EntryDate >= query.FromDate.Value);
        }
        
        if (query.ToDate.HasValue)
        {
            queryable = queryable.Where(x => x.EntryDate <= query.ToDate.Value);
        }
        
        var totalCount = await queryable.CountAsync(ct);
        
        var items = await queryable
            .OrderByDescending(x => x.EntryDate)
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .Select(x => new JournalEntrySummaryDto(
                x.Id,
                x.EntryNumber,
                x.EntryDate,
                x.EntryType,
                x.ReferenceNumber,
                x.TotalDebit,
                x.TotalCredit,
                x.Status,
                x.IsActive))
            .ToListAsync(ct);
        
        return new JournalEntriesPagedResponse(items, totalCount, query.Page, query.PageSize);
    }
}
