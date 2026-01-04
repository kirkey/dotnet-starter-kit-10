using Accounting.Application.Reports.DeferredRevenue.v1.Services;

namespace Accounting.Application.Reports.DeferredRevenue.v1;

public record GenerateDeferredRevenueReportCommand() : IRequest<byte[]>;

public sealed class GenerateDeferredRevenueReportHandler : IRequestHandler<GenerateDeferredRevenueReportCommand, byte[]>
{
    private readonly IDeferredRevenueReportService _reportService;
    public GenerateDeferredRevenueReportHandler(IDeferredRevenueReportService reportService) => _reportService = reportService;
    public async Task<byte[]> Handle(GenerateDeferredRevenueReportCommand request, CancellationToken ct) 
        => await _reportService.GenerateReportAsync();
}
