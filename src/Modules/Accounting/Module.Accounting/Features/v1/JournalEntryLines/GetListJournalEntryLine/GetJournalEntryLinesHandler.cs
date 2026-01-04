using FSH.Module.Accounting.Contracts.v1.JournalEntryLines;
using FSH.Module.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Module.Accounting.Features.v1.JournalEntryLines.GetListJournalEntryLine;

public record GetJournalEntryLinesQuery(
    int Page = 1,
    int PageSize = 10,
    string? SearchTerm = null,
    bool? IsActive = null,
    Guid? JournalEntryId = null) : IQuery<JournalEntryLinesPagedResponse>;

public record JournalEntryLinesPagedResponse(
    List<JournalEntryLineSummaryDto> Items,
    int TotalCount,
    int Page,
    int PageSize);

public class GetJournalEntryLinesHandler(AccountingDbContext context) 
    : IQueryHandler<GetJournalEntryLinesQuery, JournalEntryLinesPagedResponse>
{
    public async ValueTask<JournalEntryLinesPagedResponse> Handle(GetJournalEntryLinesQuery query, CancellationToken ct)
    {
        var queryable = context.JournalEntryLines.AsQueryable();
        
        if (query.JournalEntryId.HasValue)
        {
            queryable = queryable.Where(x => x.JournalEntryId == query.JournalEntryId.Value);
        }
        
        if (!string.IsNullOrWhiteSpace(query.SearchTerm))
        {
            queryable = queryable.Where(x => 
                x.AccountCode.Contains(query.SearchTerm) ||
                x.AccountName.Contains(query.SearchTerm));
        }
        
        if (query.IsActive.HasValue)
        {
            queryable = queryable.Where(x => x.IsActive == query.IsActive.Value);
        }
        
        var totalCount = await queryable.CountAsync(ct);
        
        var items = await queryable
            .OrderBy(x => x.LineNumber)
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .Select(x => new JournalEntryLineSummaryDto(
                x.Id,
                x.LineNumber,
                x.AccountCode,
                x.AccountName,
                x.Debit,
                x.Credit,
                x.IsActive))
            .ToListAsync(ct);
        
        return new JournalEntryLinesPagedResponse(items, totalCount, query.Page, query.PageSize);
    }
}
