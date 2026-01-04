using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Data;
using Mediator;
using Accounting.Application.Reports.TrialBalance.v1.Services;
using Microsoft.EntityFrameworkCore;

namespace FSH.Module.Accounting.Features.v1.TrialBalance.GenerateTrialBalance;

/// <summary>
/// Command to generate or refresh a Trial Balance report for a specified TrialBalance entity.
/// </summary>
/// <param name="Id">TrialBalance entity Id</param>
public record GenerateTrialBalanceCommand(Guid Id) : ICommand;

/// <summary>
/// Handler for generating a Trial Balance report using the trial balance report service.
/// </summary>
/// <remarks>
/// Responsibility: Validate TrialBalance entity presence and invoke report generation logic (AsOf date currently set to now).
/// 
/// Execution Flow:
/// 1. Validate TrialBalance entity exists
/// 2. Call reportService.GenerateReportAsync(asOfDate, options)
/// 3. (Optional) Persist or return generated report (TODO)
/// 
/// Notes:
/// - The handler currently uses DateTime.UtcNow for AsOf date; consider parameterizing the command for AsOf or period
/// 
/// Permissions: Requires access to trial balance reporting (Reports privilege)
/// 
/// Exceptions:
/// - NotFoundException: Thrown if specified TrialBalance entity not found
/// - ReportGenerationException: Possible from underlying report service
/// </remarks>
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
