using FSH.Module.Accounting.Contracts.v1.RecurringJournalEntries;
using FSH.Module.Accounting.Contracts.v1.RecurringJournalEntries.GetListRecurringJournalEntry;
using FSH.Module.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Module.Accounting.Features.v1.RecurringJournalEntries.GetListRecurringJournalEntry;

public class GetRecurringJournalEntriesHandler(AccountingDbContext context) 
    : IQueryHandler<GetRecurringJournalEntriesQuery, RecurringJournalEntriesPagedResponse>
{
    public async ValueTask<RecurringJournalEntriesPagedResponse> Handle(GetRecurringJournalEntriesQuery query, CancellationToken ct)
    {
        var queryable = context.RecurringJournalEntries.AsQueryable();
        
        if (!string.IsNullOrWhiteSpace(query.SearchTerm))
        {
            queryable = queryable.Where(x => x.Name.Contains(query.SearchTerm));
        }
        
        if (query.IsActive.HasValue)
        {
            queryable = queryable.Where(x => x.IsActive == query.IsActive.Value);
        }
        
        if (!string.IsNullOrWhiteSpace(query.Frequency))
        {
            queryable = queryable.Where(x => x.Frequency == query.Frequency);
        }
        
        var totalCount = await queryable.CountAsync(ct);
        
        var items = await queryable
            .OrderByDescending(x => x.NextRunDate)
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .Select(x => new RecurringJournalEntrySummaryDto(
                x.Id,
                x.Name,
                x.Frequency,
                x.NextRunDate,
                x.IsActive))
            .ToListAsync(ct);
        
        return new RecurringJournalEntriesPagedResponse(items, totalCount, query.Page, query.PageSize);
    }
}
