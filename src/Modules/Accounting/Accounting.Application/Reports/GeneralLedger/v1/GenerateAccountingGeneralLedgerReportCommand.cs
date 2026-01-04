using Accounting.Application.Reports.GeneralLedger.v1.Services;

namespace Accounting.Application.Reports.GeneralLedger.v1;

/// <summary>
/// Command to generate a General Ledger PDF report.
/// </summary>
public record GenerateAccountingGeneralLedgerReportCommand(
    DateTime StartDate, 
    DateTime EndDate, 
    Guid? AccountId) : IRequest<byte[]>;

/// <summary>
/// Handler for generating General Ledger PDF reports.
/// </summary>
public sealed class GenerateAccountingGeneralLedgerReportHandler 
    : IRequestHandler<GenerateAccountingGeneralLedgerReportCommand, byte[]>
{
    private readonly IGeneralLedgerReportService _reportService;

    public GenerateAccountingGeneralLedgerReportHandler(IGeneralLedgerReportService reportService)
    {
        _reportService = reportService;
    }

    public async Task<byte[]> Handle(
        GenerateAccountingGeneralLedgerReportCommand request, 
        CancellationToken cancellationToken)
    {
        return await _reportService.GenerateReportAsync(
            request.StartDate, 
            request.EndDate, 
            request.AccountId);
    }
}
