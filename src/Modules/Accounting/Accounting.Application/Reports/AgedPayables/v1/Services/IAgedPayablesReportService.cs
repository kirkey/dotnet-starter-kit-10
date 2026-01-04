namespace Accounting.Application.Reports.AgedPayables.v1.Services;

/// <summary>
/// Service interface for generating Aged Payables PDF reports using QuestPDF.
/// </summary>
public interface IAgedPayablesReportService
{
    /// <summary>
    /// Generates an Aged Payables report as of the specified date.
    /// </summary>
    /// <param name="asOfDate">The date to calculate aging from.</param>
    /// <param name="vendorId">Optional filter for a specific vendor.</param>
    /// <returns>PDF file as byte array.</returns>
    Task<byte[]> GenerateReportAsync(DateTime asOfDate, Guid? vendorId = null);
}
