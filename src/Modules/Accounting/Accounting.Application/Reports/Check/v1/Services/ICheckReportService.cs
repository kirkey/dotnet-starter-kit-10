namespace Accounting.Application.Reports.Check.v1.Services;

/// <summary>
/// Service interface for generating Check Printing PDF reports using QuestPDF.
/// </summary>
public interface ICheckReportService
{
    /// <summary>
    /// Generates a printable Check PDF report.
    /// </summary>
    /// <param name="checkId">The check ID to generate the report for.</param>
    /// <returns>PDF file as byte array.</returns>
    Task<byte[]> GenerateReportAsync(DefaultIdType checkId);
}
