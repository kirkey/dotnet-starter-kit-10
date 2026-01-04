using Accounting.Application.Reports.VarianceAnalysis.v1.Services;

namespace Accounting.Application.Reports.VarianceAnalysis.v1;

public record GenerateVarianceAnalysisReportCommand() : IRequest<byte[]>;

public sealed class GenerateVarianceAnalysisReportHandler : IRequestHandler<GenerateVarianceAnalysisReportCommand, byte[]>
{
    private readonly IVarianceAnalysisReportService _reportService;
    public GenerateVarianceAnalysisReportHandler(IVarianceAnalysisReportService reportService) => _reportService = reportService;
    public async Task<byte[]> Handle(GenerateVarianceAnalysisReportCommand request, CancellationToken ct) 
        => await _reportService.GenerateReportAsync();
}
