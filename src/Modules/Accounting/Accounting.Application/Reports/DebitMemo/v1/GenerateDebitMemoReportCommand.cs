using Accounting.Application.Reports.DebitMemo.v1.Services;

namespace Accounting.Application.Reports.DebitMemo.v1;

/// <summary>
/// Command to generate an individual Debit Memo PDF report.
/// </summary>
public record GenerateDebitMemoReportCommand(
    DefaultIdType DebitMemoId) : IRequest<byte[]>;

/// <summary>
/// Handler for generating Debit Memo PDF reports.
/// </summary>
public sealed class GenerateDebitMemoReportHandler 
    : IRequestHandler<GenerateDebitMemoReportCommand, byte[]>
{
    private readonly IDebitMemoReportService _reportService;

    public GenerateDebitMemoReportHandler(IDebitMemoReportService reportService)
    {
        _reportService = reportService;
    }

    public async Task<byte[]> Handle(
        GenerateDebitMemoReportCommand request, 
        CancellationToken cancellationToken)
    {
        return await _reportService.GenerateReportAsync(request.DebitMemoId);
    }
}
