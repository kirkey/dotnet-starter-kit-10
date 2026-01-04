namespace Accounting.Application.Reports.AgedReceivables.v1.Services;

/// <summary>
/// Service interface for generating Aged Receivables PDF reports using QuestPDF.
/// </summary>
public interface IAgedReceivablesReportService
{
    /// <summary>
    /// Generates an Aged Receivables report as of the specified date.
    /// </summary>
    /// <param name="asOfDate">The date to calculate aging from.</param>
    /// <param name="customerId">Optional filter for a specific customer.</param>
    /// <returns>PDF file as byte array.</returns>
    Task<byte[]> GenerateReportAsync(DateTime asOfDate, Guid? customerId = null);
}
