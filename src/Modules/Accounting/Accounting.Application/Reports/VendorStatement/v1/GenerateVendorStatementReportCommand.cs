using Accounting.Application.Reports.VendorStatement.v1.Services;

namespace Accounting.Application.Reports.VendorStatement.v1;

/// <summary>
/// Command to generate a Vendor Statement PDF report.
/// </summary>
public record GenerateVendorStatementReportCommand(
    DefaultIdType VendorId,
    DateTime StartDate,
    DateTime EndDate) : IRequest<byte[]>;

/// <summary>
/// Handler for generating Vendor Statement PDF reports.
/// </summary>
public sealed class GenerateVendorStatementReportHandler 
    : IRequestHandler<GenerateVendorStatementReportCommand, byte[]>
{
    private readonly IVendorStatementReportService _reportService;

    public GenerateVendorStatementReportHandler(IVendorStatementReportService reportService)
    {
        _reportService = reportService;
    }

    public async Task<byte[]> Handle(
        GenerateVendorStatementReportCommand request, 
        CancellationToken cancellationToken)
    {
        return await _reportService.GenerateReportAsync(
            request.VendorId,
            request.StartDate,
            request.EndDate);
    }
}
