using Accounting.Application.Reports.Bill.v1.Services;

namespace Accounting.Application.Reports.Bill.v1;

/// <summary>
/// Command to generate an individual Bill PDF report.
/// </summary>
public record GenerateBillReportCommand(
    DefaultIdType BillId) : IRequest<byte[]>;

/// <summary>
/// Handler for generating Bill PDF reports.
/// </summary>
public sealed class GenerateBillReportHandler 
    : IRequestHandler<GenerateBillReportCommand, byte[]>
{
    private readonly IBillReportService _reportService;

    public GenerateBillReportHandler(IBillReportService reportService)
    {
        _reportService = reportService;
    }

    public async Task<byte[]> Handle(
        GenerateBillReportCommand request, 
        CancellationToken cancellationToken)
    {
        return await _reportService.GenerateReportAsync(request.BillId);
    }
}
