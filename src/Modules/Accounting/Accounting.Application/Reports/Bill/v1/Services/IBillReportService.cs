namespace Accounting.Application.Reports.Bill.v1.Services;

/// <summary>
/// Service interface for generating individual Bill PDF reports using QuestPDF.
/// </summary>
public interface IBillReportService
{
    /// <summary>
    /// Generates an individual Bill PDF report.
    /// </summary>
    /// <param name="billId">The bill ID to generate the report for.</param>
    /// <returns>PDF file as byte array.</returns>
    Task<byte[]> GenerateReportAsync(DefaultIdType billId);
}
