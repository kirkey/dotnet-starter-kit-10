using Accounting.Application.Reports.BalanceSheet.v1.Services;

namespace Accounting.Application.Reports.BalanceSheet.v1;

/// <summary>
/// Command to generate a Balance Sheet PDF report.
/// </summary>
public record GenerateBalanceSheetReportCommand(
    DateTime AsOfDate,
    bool IncludeComparative,
    DateTime? ComparativeAsOfDate) : IRequest<byte[]>;

/// <summary>
/// Handler for generating Balance Sheet PDF reports.
/// </summary>
public sealed class GenerateBalanceSheetReportHandler 
    : IRequestHandler<GenerateBalanceSheetReportCommand, byte[]>
{
    private readonly IBalanceSheetReportService _reportService;

    public GenerateBalanceSheetReportHandler(IBalanceSheetReportService reportService)
    {
        _reportService = reportService;
    }

    public async Task<byte[]> Handle(
        GenerateBalanceSheetReportCommand request, 
        CancellationToken cancellationToken)
    {
        return await _reportService.GenerateReportAsync(
            request.AsOfDate,
            request.IncludeComparative,
            request.ComparativeAsOfDate);
    }
}
