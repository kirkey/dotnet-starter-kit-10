using Accounting.Application.Reports.CashFlowStatement.v1.Services;

namespace Accounting.Application.Reports.CashFlowStatement.v1;

/// <summary>
/// Command to generate a Cash Flow Statement PDF report.
/// </summary>
public record GenerateCashFlowStatementReportCommand(
    DateTime StartDate,
    DateTime EndDate,
    bool IncludeComparative,
    DateTime? ComparativeStartDate,
    DateTime? ComparativeEndDate) : IRequest<byte[]>;

/// <summary>
/// Handler for generating Cash Flow Statement PDF reports.
/// </summary>
public sealed class GenerateCashFlowStatementReportHandler 
    : IRequestHandler<GenerateCashFlowStatementReportCommand, byte[]>
{
    private readonly ICashFlowStatementReportService _reportService;

    public GenerateCashFlowStatementReportHandler(ICashFlowStatementReportService reportService)
    {
        _reportService = reportService;
    }

    public async Task<byte[]> Handle(
        GenerateCashFlowStatementReportCommand request, 
        CancellationToken cancellationToken)
    {
        return await _reportService.GenerateReportAsync(
            request.StartDate,
            request.EndDate,
            request.IncludeComparative,
            request.ComparativeStartDate,
            request.ComparativeEndDate);
    }
}
