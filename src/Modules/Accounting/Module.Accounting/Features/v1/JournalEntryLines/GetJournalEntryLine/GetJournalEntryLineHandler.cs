using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Contracts.v1.JournalEntryLines;
using FSH.Module.Accounting.Contracts.v1.JournalEntryLines.GetJournalEntryLine;
using FSH.Module.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Module.Accounting.Features.v1.JournalEntryLines.GetJournalEntryLine;


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
