using Accounting.Application.Reports.BankReconciliation.v1.Services;

namespace Accounting.Application.Reports.BankReconciliation.v1;

public record GenerateBankReconciliationReportCommand(
    DefaultIdType ReconciliationId) : IRequest<byte[]>;

public sealed class GenerateBankReconciliationReportHandler 
    : IRequestHandler<GenerateBankReconciliationReportCommand, byte[]>
{
    private readonly IBankReconciliationReportService _reportService;

    public GenerateBankReconciliationReportHandler(IBankReconciliationReportService reportService)
    {
        _reportService = reportService;
    }

    public async Task<byte[]> Handle(
        GenerateBankReconciliationReportCommand request, 
        CancellationToken cancellationToken)
    {
        return await _reportService.GenerateReportAsync(request.ReconciliationId);
    }
}
