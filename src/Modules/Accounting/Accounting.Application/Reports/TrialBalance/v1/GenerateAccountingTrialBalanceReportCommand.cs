using Accounting.Application.Reports.TrialBalance.v1.Services;

namespace Accounting.Application.Reports.TrialBalance.v1;

/// <summary>
/// Command to generate a Trial Balance PDF report.
/// </summary>
public record GenerateAccountingTrialBalanceReportCommand(
    DateTime AsOfDate, 
    Guid? PeriodId) : IRequest<byte[]>;

/// <summary>
/// Handler for generating Trial Balance PDF reports.
/// </summary>
public sealed class GenerateAccountingTrialBalanceReportHandler 
    : IRequestHandler<GenerateAccountingTrialBalanceReportCommand, byte[]>
{
    private readonly ITrialBalanceReportService _reportService;

    public GenerateAccountingTrialBalanceReportHandler(ITrialBalanceReportService reportService)
    {
        _reportService = reportService;
    }

    public async Task<byte[]> Handle(
        GenerateAccountingTrialBalanceReportCommand request, 
        CancellationToken cancellationToken)
    {
        return await _reportService.GenerateReportAsync(
            request.AsOfDate, 
            request.PeriodId);
    }
}
