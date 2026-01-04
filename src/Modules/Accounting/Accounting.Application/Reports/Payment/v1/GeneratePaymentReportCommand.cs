using Accounting.Application.Reports.Payment.v1.Services;

namespace Accounting.Application.Reports.Payment.v1;

/// <summary>
/// Command to generate a Payment Receipt PDF report.
/// </summary>
public record GeneratePaymentReportCommand(
    DefaultIdType PaymentId) : IRequest<byte[]>;

/// <summary>
/// Handler for generating Payment Receipt PDF reports.
/// </summary>
public sealed class GeneratePaymentReportHandler 
    : IRequestHandler<GeneratePaymentReportCommand, byte[]>
{
    private readonly IPaymentReportService _reportService;

    public GeneratePaymentReportHandler(IPaymentReportService reportService)
    {
        _reportService = reportService;
    }

    public async Task<byte[]> Handle(
        GeneratePaymentReportCommand request, 
        CancellationToken cancellationToken)
    {
        return await _reportService.GenerateReportAsync(request.PaymentId);
    }
}
