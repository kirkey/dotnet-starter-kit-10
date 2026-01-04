using Accounting.Application.Reports.ChartOfAccountsStructure.v1.Services;

namespace Accounting.Application.Reports.ChartOfAccountsStructure.v1;

public record GenerateChartOfAccountsStructureReportCommand() : IRequest<byte[]>;

public sealed class GenerateChartOfAccountsStructureReportHandler : IRequestHandler<GenerateChartOfAccountsStructureReportCommand, byte[]>
{
    private readonly IChartOfAccountsStructureReportService _reportService;
    public GenerateChartOfAccountsStructureReportHandler(IChartOfAccountsStructureReportService reportService) => _reportService = reportService;
    public async Task<byte[]> Handle(GenerateChartOfAccountsStructureReportCommand request, CancellationToken ct) 
        => await _reportService.GenerateReportAsync();
}
