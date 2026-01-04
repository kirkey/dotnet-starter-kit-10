using Accounting.Application.Reports.PeriodStatus.v1.Services;

namespace Accounting.Application.Reports.PeriodStatus.v1;

public record GeneratePeriodStatusReportCommand() : IRequest<byte[]>;

public sealed class GeneratePeriodStatusReportHandler : IRequestHandler<GeneratePeriodStatusReportCommand, byte[]>
{
    private readonly IPeriodStatusReportService _reportService;
    public GeneratePeriodStatusReportHandler(IPeriodStatusReportService reportService) => _reportService = reportService;
    public async Task<byte[]> Handle(GeneratePeriodStatusReportCommand request, CancellationToken ct) 
        => await _reportService.GenerateReportAsync();
}
