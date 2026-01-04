using FSH.Framework.Core.Exceptions;
using FSH.Modules.Accounting.Data;
using Mediator;
using Accounting.Application.Reports.TrialBalance.v1.Services;
using Microsoft.EntityFrameworkCore;

namespace FSH.Modules.Accounting.Features.v1.TrialBalance.GenerateTrialBalance;

public record GenerateTrialBalanceCommand(Guid Id) : ICommand;

public class GenerateTrialBalanceHandler(AccountingDbContext context, ITrialBalanceReportService reportService) 
    : ICommandHandler<GenerateTrialBalanceCommand>
{
    public async ValueTask<Unit> Handle(GenerateTrialBalanceCommand command, CancellationToken ct)
    {
        var entity = await context.TrialBalance.FirstOrDefaultAsync(x => x.Id == command.Id, ct)
            ?? throw new NotFoundException("TrialBalance not found");

        // Generate report as of now (can be extended to accept AsOfDate/PeriodId in the command)
        var _ = await reportService.GenerateReportAsync(DateTime.UtcNow, null);
        // TODO: persist or return generated report if required by caller

        return Unit.Value;
    }
}
