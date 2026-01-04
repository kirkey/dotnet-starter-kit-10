using Accounting.Application.Reports.CustomerStatement.v1.Services;

namespace Accounting.Application.Reports.CustomerStatement.v1;

/// <summary>
/// Command to generate a Customer Statement PDF report.
/// </summary>
public record GenerateCustomerStatementReportCommand(
    DefaultIdType CustomerId,
    DateTime StartDate,
    DateTime EndDate) : IRequest<byte[]>;

/// <summary>
/// Handler for generating Customer Statement PDF reports.
/// </summary>
public sealed class GenerateCustomerStatementReportHandler 
    : IRequestHandler<GenerateCustomerStatementReportCommand, byte[]>
{
    private readonly ICustomerStatementReportService _reportService;

    public GenerateCustomerStatementReportHandler(ICustomerStatementReportService reportService)
    {
        _reportService = reportService;
    }

    public async Task<byte[]> Handle(
        GenerateCustomerStatementReportCommand request, 
        CancellationToken cancellationToken)
    {
        return await _reportService.GenerateReportAsync(
            request.CustomerId,
            request.StartDate,
            request.EndDate);
    }
}
