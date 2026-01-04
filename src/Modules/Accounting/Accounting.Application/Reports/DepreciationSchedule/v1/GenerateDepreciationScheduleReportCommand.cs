using Accounting.Application.Reports.DepreciationSchedule.v1.Services;

namespace Accounting.Application.Reports.DepreciationSchedule.v1;

public record GenerateDepreciationScheduleReportCommand() : IRequest<byte[]>;

public sealed class GenerateDepreciationScheduleReportHandler : IRequestHandler<GenerateDepreciationScheduleReportCommand, byte[]>
{
    private readonly IDepreciationScheduleReportService _reportService;
    public GenerateDepreciationScheduleReportHandler(IDepreciationScheduleReportService reportService) => _reportService = reportService;
    public async Task<byte[]> Handle(GenerateDepreciationScheduleReportCommand request, CancellationToken ct) 
        => await _reportService.GenerateReportAsync();
}
