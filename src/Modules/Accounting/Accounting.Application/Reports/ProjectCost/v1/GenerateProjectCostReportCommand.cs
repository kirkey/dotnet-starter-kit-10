using Accounting.Application.Reports.ProjectCost.v1.Services;

namespace Accounting.Application.Reports.ProjectCost.v1;

public record GenerateProjectCostReportCommand() : IRequest<byte[]>;

public sealed class GenerateProjectCostReportHandler : IRequestHandler<GenerateProjectCostReportCommand, byte[]>
{
    private readonly IProjectCostReportService _reportService;
    public GenerateProjectCostReportHandler(IProjectCostReportService reportService) => _reportService = reportService;
    public async Task<byte[]> Handle(GenerateProjectCostReportCommand request, CancellationToken ct) 
        => await _reportService.GenerateReportAsync();
}
