using FSH.Framework.Core.Exceptions;
using FSH.Modules.Accounting.Contracts.v1.JournalEntries;
using FSH.Modules.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Modules.Accounting.Features.v1.JournalEntries.GetJournalEntry;

public record GetJournalEntryQuery(Guid Id) : IQuery<JournalEntryDto>;

public class GetJournalEntryHandler(AccountingDbContext context) : IQueryHandler<GetJournalEntryQuery, JournalEntryDto>
{
    public async ValueTask<JournalEntryDto> Handle(GetJournalEntryQuery query, CancellationToken ct)
    {
        var entity = await context.JournalEntries
            .Where(x => x.Id == query.Id)
            .Select(x => new JournalEntryDto(
                x.Id,
                x.EntryNumber,
                x.EntryDate,
                x.EntryType,
                x.ReferenceNumber,
                x.ReferenceType,
                x.TotalDebit,
                x.TotalCredit,
                x.FiscalPeriodId,
                x.Status,
                x.PostedDate,
                x.PostedBy,
                x.ApprovedDate,
                x.ApprovedBy,
                x.IsReversed,
                x.ReversedEntryId,
                x.ReversedDate,
                x.Description,
                x.Notes,
                x.Memo,
                x.IsActive,
                x.CreatedOnUtc))
            .FirstOrDefaultAsync(ct);
        
        return entity ?? throw new NotFoundException("JournalEntry not found");
    }
}
