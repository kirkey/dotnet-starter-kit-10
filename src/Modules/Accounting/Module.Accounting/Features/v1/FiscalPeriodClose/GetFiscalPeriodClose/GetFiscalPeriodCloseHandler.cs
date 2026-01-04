using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Contracts.v1.FiscalPeriodClose;
using FSH.Module.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Module.Accounting.Features.v1.FiscalPeriodClose.GetFiscalPeriodClose;

public record GetFiscalPeriodCloseByIdQuery(Guid Id) : IQuery<FiscalPeriodCloseDto>;

public class GetFiscalPeriodCloseByIdHandler(AccountingDbContext context) : IQueryHandler<GetFiscalPeriodCloseByIdQuery, FiscalPeriodCloseDto>
{
    public async ValueTask<FiscalPeriodCloseDto> Handle(GetFiscalPeriodCloseByIdQuery query, CancellationToken ct)
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
