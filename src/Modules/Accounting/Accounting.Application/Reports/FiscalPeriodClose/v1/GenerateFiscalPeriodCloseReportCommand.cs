using Accounting.Application.Reports.FiscalPeriodClose.v1.Services;

namespace Accounting.Application.Reports.FiscalPeriodClose.v1;

public record GenerateFiscalPeriodCloseReportCommand() : IRequest<byte[]>;

public sealed class GenerateFiscalPeriodCloseReportHandler : IRequestHandler<GenerateFiscalPeriodCloseReportCommand, byte[]>
{
    private readonly IFiscalPeriodCloseReportService _reportService;
    public GenerateFiscalPeriodCloseReportHandler(IFiscalPeriodCloseReportService reportService) => _reportService = reportService;
    public async Task<byte[]> Handle(GenerateFiscalPeriodCloseReportCommand request, CancellationToken ct) 
        => await _reportService.GenerateReportAsync();
}
