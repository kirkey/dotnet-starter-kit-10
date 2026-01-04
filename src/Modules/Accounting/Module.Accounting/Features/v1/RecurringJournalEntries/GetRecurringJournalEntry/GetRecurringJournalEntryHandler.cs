using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Contracts.v1.RecurringJournalEntries;
using FSH.Module.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Module.Accounting.Features.v1.RecurringJournalEntries.GetRecurringJournalEntry;

public record GetRecurringJournalEntryQuery(Guid Id) : IQuery<RecurringJournalEntryDto>;

public class GetRecurringJournalEntryHandler(AccountingDbContext context) : IQueryHandler<GetRecurringJournalEntryQuery, RecurringJournalEntryDto>
{
    public async ValueTask<RecurringJournalEntryDto> Handle(GetRecurringJournalEntryQuery query, CancellationToken ct)
    {
        var entity = await context.RecurringJournalEntries
            .Where(x => x.Id == query.Id)
            .Select(x => new RecurringJournalEntryDto(
                x.Id,
                x.Name,
                x.Frequency,
                x.NextRunDate,
                x.LastRunDate,
                x.FiscalPeriodId,
                x.IsAutoPost,
                x.Description,
                x.IsActive,
                x.CreatedOnUtc))
            .FirstOrDefaultAsync(ct);
        
        return entity ?? throw new NotFoundException("RecurringJournalEntry not found");
    }
}
