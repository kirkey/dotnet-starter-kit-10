using Accounting.Application.Reports.AgedReceivables.v1.Services;

namespace Accounting.Application.Reports.AgedReceivables.v1;

/// <summary>
/// Command to generate an Aged Receivables PDF report.
/// </summary>
public record GenerateAccountingAgedReceivablesReportCommand(
    DateTime AsOfDate, 
    Guid? CustomerId) : IRequest<byte[]>;

/// <summary>
/// Handler for generating Aged Receivables PDF reports.
/// </summary>
public sealed class GenerateAccountingAgedReceivablesReportHandler 
    : IRequestHandler<GenerateAccountingAgedReceivablesReportCommand, byte[]>
{
    private readonly IAgedReceivablesReportService _reportService;

    public GenerateAccountingAgedReceivablesReportHandler(IAgedReceivablesReportService reportService)
    {
        _reportService = reportService;
    }

    public async Task<byte[]> Handle(
        GenerateAccountingAgedReceivablesReportCommand request, 
        CancellationToken cancellationToken)
    {
        return await _reportService.GenerateReportAsync(
            request.AsOfDate, 
            request.CustomerId);
    }
}
