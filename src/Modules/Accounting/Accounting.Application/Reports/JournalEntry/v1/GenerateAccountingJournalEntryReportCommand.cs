using Accounting.Application.Reports.JournalEntry.v1.Services;

namespace Accounting.Application.Reports.JournalEntry.v1;

/// <summary>
/// Command to generate a Journal Entry PDF report.
/// </summary>
public record GenerateAccountingJournalEntryReportCommand(
    DateTime StartDate, 
    DateTime EndDate, 
    bool IsPostedOnly) : IRequest<byte[]>;

/// <summary>
/// Handler for generating Journal Entry PDF reports.
/// </summary>
public sealed class GenerateAccountingJournalEntryReportHandler 
    : IRequestHandler<GenerateAccountingJournalEntryReportCommand, byte[]>
{
    private readonly IJournalEntryReportService _reportService;

    public GenerateAccountingJournalEntryReportHandler(IJournalEntryReportService reportService)
    {
        _reportService = reportService;
    }

    public async Task<byte[]> Handle(
        GenerateAccountingJournalEntryReportCommand request, 
        CancellationToken cancellationToken)
    {
        return await _reportService.GenerateReportAsync(
            request.StartDate, 
            request.EndDate, 
            request.IsPostedOnly);
    }
}
