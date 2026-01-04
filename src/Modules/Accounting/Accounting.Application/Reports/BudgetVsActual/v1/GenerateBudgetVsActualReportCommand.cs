using Accounting.Application.Reports.BudgetVsActual.v1.Services;

namespace Accounting.Application.Reports.BudgetVsActual.v1;

public record GenerateBudgetVsActualReportCommand() : IRequest<byte[]>;

public sealed class GenerateBudgetVsActualReportHandler : IRequestHandler<GenerateBudgetVsActualReportCommand, byte[]>
{
    private readonly IBudgetVsActualReportService _reportService;
    public GenerateBudgetVsActualReportHandler(IBudgetVsActualReportService reportService) => _reportService = reportService;
    public async Task<byte[]> Handle(GenerateBudgetVsActualReportCommand request, CancellationToken ct) 
        => await _reportService.GenerateReportAsync();
}
