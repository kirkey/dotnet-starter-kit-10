using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Contracts.v1.FiscalPeriodClose;
using FSH.Module.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Module.Accounting.Features.v1.FiscalPeriodClose.GetFiscalPeriodClose;

/// <summary>
/// Query to retrieve a single FiscalPeriodClose by Id.
/// </summary>
/// <param name="Id">FiscalPeriodClose ID to retrieve</param>
public record GetFiscalPeriodCloseByIdQuery(Guid Id) : IQuery<FiscalPeriodCloseDto>;

/// <summary>
/// Handler for retrieving a FiscalPeriodClose with essential metadata for review prior to completion.
/// </summary>
/// <remarks>
/// Responsibility: Project FiscalPeriodClose to DTO with period dates, status, closing journal reference and audit details.
/// 
/// Execution Flow:
/// 1. Query FiscalPeriodClose DbSet by Id and project to FiscalPeriodCloseDto
/// 2. Throw NotFoundException if record missing
/// 
/// Use Cases: Review period details, verify CloseDate and ClosingJournalEntryId after completion
/// 
/// Permissions: Requires FiscalPeriodClose.View
/// 
/// Exceptions:
/// - NotFoundException: Thrown if the specified period close is not found
/// </remarks>
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
