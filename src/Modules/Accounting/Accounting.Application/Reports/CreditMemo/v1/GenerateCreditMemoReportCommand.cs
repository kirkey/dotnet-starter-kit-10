using Accounting.Application.Reports.CreditMemo.v1.Services;

namespace Accounting.Application.Reports.CreditMemo.v1;

/// <summary>
/// Command to generate an individual Credit Memo PDF report.
/// </summary>
public record GenerateCreditMemoReportCommand(
    DefaultIdType CreditMemoId) : IRequest<byte[]>;

/// <summary>
/// Handler for generating Credit Memo PDF reports.
/// </summary>
public sealed class GenerateCreditMemoReportHandler 
    : IRequestHandler<GenerateCreditMemoReportCommand, byte[]>
{
    private readonly ICreditMemoReportService _reportService;

    public GenerateCreditMemoReportHandler(ICreditMemoReportService reportService)
    {
        _reportService = reportService;
    }

    public async Task<byte[]> Handle(
        GenerateCreditMemoReportCommand request, 
        CancellationToken cancellationToken)
    {
        return await _reportService.GenerateReportAsync(request.CreditMemoId);
    }
}
