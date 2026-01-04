using Accounting.Application.Reports.Invoice.v1.Services;

namespace Accounting.Application.Reports.Invoice.v1;

/// <summary>
/// Command to generate an individual Invoice PDF report.
/// </summary>
public record GenerateInvoiceReportCommand(
    DefaultIdType InvoiceId) : IRequest<byte[]>;

/// <summary>
/// Handler for generating Invoice PDF reports.
/// </summary>
public sealed class GenerateInvoiceReportHandler 
    : IRequestHandler<GenerateInvoiceReportCommand, byte[]>
{
    private readonly IInvoiceReportService _reportService;

    public GenerateInvoiceReportHandler(IInvoiceReportService reportService)
    {
        _reportService = reportService;
    }

    public async Task<byte[]> Handle(
        GenerateInvoiceReportCommand request, 
        CancellationToken cancellationToken)
    {
        return await _reportService.GenerateReportAsync(request.InvoiceId);
    }
}
