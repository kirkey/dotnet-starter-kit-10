using Accounting.Application.Reports.IncomeStatement.v1.Services;

namespace Accounting.Application.Reports.IncomeStatement.v1;

/// <summary>
/// Command to generate an Income Statement PDF report.
/// </summary>
public record GenerateIncomeStatementReportCommand(
    DateTime StartDate,
    DateTime EndDate,
    bool IncludeComparative,
    DateTime? ComparativeStartDate,
    DateTime? ComparativeEndDate) : IRequest<byte[]>;

/// <summary>
/// Handler for generating Income Statement PDF reports.
/// </summary>
public sealed class GenerateIncomeStatementReportHandler 
    : IRequestHandler<GenerateIncomeStatementReportCommand, byte[]>
{
    private readonly IIncomeStatementReportService _reportService;

    public GenerateIncomeStatementReportHandler(IIncomeStatementReportService reportService)
    {
        _reportService = reportService;
    }

    public async Task<byte[]> Handle(
        GenerateIncomeStatementReportCommand request, 
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
