using FSH.Framework.Core.Exceptions;
using FSH.Modules.Accounting.Contracts.v1.JournalEntryLines;
using FSH.Modules.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Modules.Accounting.Features.v1.JournalEntryLines.GetJournalEntryLine;

public record GetJournalEntryLineQuery(Guid Id) : IQuery<JournalEntryLineDto>;

public class GetJournalEntryLineHandler(AccountingDbContext context) : IQueryHandler<GetJournalEntryLineQuery, JournalEntryLineDto>
{
    public async ValueTask<JournalEntryLineDto> Handle(GetJournalEntryLineQuery query, CancellationToken ct)
    {
        var entity = await context.JournalEntryLines
            .Where(x => x.Id == query.Id)
            .Select(x => new JournalEntryLineDto(
                x.Id,
                x.JournalEntryId,
                x.LineNumber,
                x.AccountId,
                x.AccountCode,
                x.AccountName,
                x.Debit,
                x.Credit,
                x.Amount,
                x.TransactionType,
                x.ReferenceNumber,
                x.Description,
                x.Notes,
                x.CostCenterId,
                x.DepartmentId,
                x.ProjectId,
                x.IsActive,
                x.CreatedOnUtc))
            .FirstOrDefaultAsync(ct);
        
        return entity ?? throw new NotFoundException("JournalEntryLine not found");
    }
}
