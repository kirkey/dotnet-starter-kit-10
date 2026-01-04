using Accounting.Application.Reports.BankAccountSummary.v1.Services;

namespace Accounting.Application.Reports.BankAccountSummary.v1;

public record GenerateBankAccountSummaryReportCommand() : IRequest<byte[]>;

public sealed class GenerateBankAccountSummaryReportHandler : IRequestHandler<GenerateBankAccountSummaryReportCommand, byte[]>
{
    private readonly IBankAccountSummaryReportService _reportService;
    public GenerateBankAccountSummaryReportHandler(IBankAccountSummaryReportService reportService) => _reportService = reportService;
    public async Task<byte[]> Handle(GenerateBankAccountSummaryReportCommand request, CancellationToken ct) 
        => await _reportService.GenerateReportAsync();
}
