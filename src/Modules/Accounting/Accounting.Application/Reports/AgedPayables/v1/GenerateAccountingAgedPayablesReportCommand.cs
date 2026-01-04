using Accounting.Application.Reports.AgedPayables.v1.Services;

namespace Accounting.Application.Reports.AgedPayables.v1;

/// <summary>
/// Command to generate an Aged Payables PDF report.
/// </summary>
public record GenerateAccountingAgedPayablesReportCommand(
    DateTime AsOfDate, 
    Guid? VendorId) : IRequest<byte[]>;

/// <summary>
/// Handler for generating Aged Payables PDF reports.
/// </summary>
public sealed class GenerateAccountingAgedPayablesReportHandler 
    : IRequestHandler<GenerateAccountingAgedPayablesReportCommand, byte[]>
{
    private readonly IAgedPayablesReportService _reportService;

    public GenerateAccountingAgedPayablesReportHandler(IAgedPayablesReportService reportService)
    {
        _reportService = reportService;
    }

    public async Task<byte[]> Handle(
        GenerateAccountingAgedPayablesReportCommand request, 
        CancellationToken cancellationToken)
    {
        return await _reportService.GenerateReportAsync(
            request.AsOfDate, 
            request.VendorId);
    }
}
