namespace Accounting.Application.Reports.DebitMemo.v1.Services;

/// <summary>
/// Service interface for generating individual Debit Memo PDF reports using QuestPDF.
/// </summary>
public interface IDebitMemoReportService
{
    /// <summary>
    /// Generates an individual Debit Memo PDF report.
    /// </summary>
    /// <param name="debitMemoId">The debit memo ID to generate the report for.</param>
    /// <returns>PDF file as byte array.</returns>
    Task<byte[]> GenerateReportAsync(DefaultIdType debitMemoId);
}
