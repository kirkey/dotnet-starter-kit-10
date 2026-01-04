using Accounting.Application.Reports.Check.v1.Services;

namespace Accounting.Application.Reports.Check.v1;

/// <summary>
/// Command to generate a printable Check PDF report.
/// </summary>
public record GenerateCheckReportCommand(
    DefaultIdType CheckId) : IRequest<byte[]>;

/// <summary>
/// Handler for generating Check PDF reports.
/// </summary>
public sealed class GenerateCheckReportHandler 
    : IRequestHandler<GenerateCheckReportCommand, byte[]>
{
    private readonly ICheckReportService _reportService;

    public GenerateCheckReportHandler(ICheckReportService reportService)
    {
        _reportService = reportService;
    }

    public async Task<byte[]> Handle(
        GenerateCheckReportCommand request, 
        CancellationToken cancellationToken)
    {
        return await _reportService.GenerateReportAsync(request.CheckId);
    }
}
