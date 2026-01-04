using FSH.Framework.Core.Exceptions;
using FSH.Modules.Accounting.Contracts.v1.Checks;
using FSH.Modules.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Modules.Accounting.Features.v1.Checks.GetCheck;

public record GetCheckQuery(Guid Id) : IQuery<CheckDto>;

public class GetCheckHandler(AccountingDbContext context) : IQueryHandler<GetCheckQuery, CheckDto>
{
    public async ValueTask<CheckDto> Handle(GetCheckQuery query, CancellationToken ct)
    {
        var entity = await context.Checks
            .Where(x => x.Id == query.Id)
            .Select(x => new CheckDto(
                x.Id,
                x.CheckNumber,
                x.CheckDate,
                x.CheckType,
                x.PayeeId,
                x.PayeeName,
                x.BankAccountId,
                x.AccountNumber,
                x.Amount,
                x.Status,
                x.PrintedDate,
                x.ClearedDate,
                x.ClearedBy,
                x.JournalEntryId,
                x.ReferenceNumber,
                x.Notes,
                x.IsActive,
                x.CreatedOnUtc))
            .FirstOrDefaultAsync(ct);
        
        return entity ?? throw new NotFoundException("Check not found");
    }
}
