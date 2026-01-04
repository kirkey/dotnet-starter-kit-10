namespace Accounting.Application.Reports.CreditMemo.v1.Services;

/// <summary>
/// Service interface for generating individual Credit Memo PDF reports using QuestPDF.
/// </summary>
public interface ICreditMemoReportService
{
    /// <summary>
    /// Generates an individual Credit Memo PDF report.
    /// </summary>
    /// <param name="creditMemoId">The credit memo ID to generate the report for.</param>
    /// <returns>PDF file as byte array.</returns>
    Task<byte[]> GenerateReportAsync(DefaultIdType creditMemoId);
}
