using Accounting.Application.Reports.WriteOffSummary.v1.Services;

namespace Accounting.Application.Reports.WriteOffSummary.v1;

public record GenerateWriteOffSummaryReportCommand() : IRequest<byte[]>;

public sealed class GenerateWriteOffSummaryReportHandler : IRequestHandler<GenerateWriteOffSummaryReportCommand, byte[]>
{
    private readonly IWriteOffSummaryReportService _reportService;
    public GenerateWriteOffSummaryReportHandler(IWriteOffSummaryReportService reportService) => _reportService = reportService;
    public async Task<byte[]> Handle(GenerateWriteOffSummaryReportCommand request, CancellationToken ct) 
        => await _reportService.GenerateReportAsync();
}
