using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Data;
using Mediator;
using Accounting.Application.Reports.GeneralLedger.v1.Services;
using Microsoft.EntityFrameworkCore;

namespace FSH.Module.Accounting.Features.v1.GeneralLedger.ExportGeneralLedger;

public record ExportGeneralLedgerCommand(Guid Id) : ICommand;

public class ExportGeneralLedgerHandler(AccountingDbContext context, IGeneralLedgerReportService reportService) 
    : ICommandHandler<ExportGeneralLedgerCommand>
{
    public async ValueTask<Unit> Handle(ExportGeneralLedgerCommand command, CancellationToken ct)
    {
        var entity = await context.GeneralLedger.FirstOrDefaultAsync(x => x.Id == command.Id, ct)
            ?? throw new NotFoundException("GeneralLedger not found");

        // Export for full history up to now. Date range could be parameterized later.
        var _ = await reportService.GenerateReportAsync(DateTime.MinValue, DateTime.UtcNow, command.Id);
        // TODO: return or persist exported bytes if required by the API

        return Unit.Value;
    }
}
