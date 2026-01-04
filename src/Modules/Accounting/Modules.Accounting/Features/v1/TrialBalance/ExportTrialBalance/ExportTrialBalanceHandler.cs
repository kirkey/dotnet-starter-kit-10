using FSH.Framework.Core.Exceptions;
using FSH.Modules.Accounting.Data;
using Mediator;
using Accounting.Application.Reports.TrialBalance.v1.Services;
using Microsoft.EntityFrameworkCore;

namespace FSH.Modules.Accounting.Features.v1.TrialBalance.ExportTrialBalance;

public record ExportTrialBalanceCommand(Guid Id) : ICommand;

public class ExportTrialBalanceHandler(AccountingDbContext context, ITrialBalanceReportService reportService) 
    : ICommandHandler<ExportTrialBalanceCommand>
{
    public async ValueTask<Unit> Handle(ExportTrialBalanceCommand command, CancellationToken ct)
    {
        var entity = await context.TrialBalance.FirstOrDefaultAsync(x => x.Id == command.Id, ct)
            ?? throw new NotFoundException("TrialBalance not found");

        // Export uses same report generation for now; could be extended to different formats
        var _ = await reportService.GenerateReportAsync(DateTime.UtcNow, null);
        // TODO: return or persist exported bytes (e.g., store in blob, attach to entity, or return in response)

        return Unit.Value;
    }
}
