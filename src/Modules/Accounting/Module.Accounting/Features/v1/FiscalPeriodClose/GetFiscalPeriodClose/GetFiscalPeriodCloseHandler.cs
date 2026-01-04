using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Contracts.v1.FiscalPeriodClose;
using FSH.Module.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Module.Accounting.Features.v1.FiscalPeriodClose.GetFiscalPeriodClose;

public record GetFiscalPeriodCloseQuery(Guid Id) : IQuery<FiscalPeriodCloseDto>;

public class GetFiscalPeriodCloseHandler(AccountingDbContext context) : IQueryHandler<GetFiscalPeriodCloseQuery, FiscalPeriodCloseDto>
{
    public async ValueTask<FiscalPeriodCloseDto> Handle(GetFiscalPeriodCloseQuery query, CancellationToken ct)
    {
        var entity = await context.FiscalPeriodClose
            .Where(x => x.Id == query.Id)
            .Select(x => new FiscalPeriodCloseDto(
                x.Id,
                x.FiscalPeriodId,
                x.FiscalYear,
                x.PeriodName,
                x.StartDate,
                x.EndDate,
                x.CloseDate,
                x.Status,
                x.RetainedEarnings,
                x.ClosingJournalEntryId,
                x.ClosedBy,
                x.Description,
                x.IsActive,
                x.CreatedOnUtc))
            .FirstOrDefaultAsync(ct);
        
        return entity ?? throw new NotFoundException("FiscalPeriodClose not found");
    }
}
