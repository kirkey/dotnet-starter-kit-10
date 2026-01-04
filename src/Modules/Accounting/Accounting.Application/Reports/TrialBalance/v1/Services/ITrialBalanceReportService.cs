namespace Accounting.Application.Reports.TrialBalance.v1.Services;

/// <summary>
/// Service interface for generating Trial Balance PDF reports using QuestPDF.
/// </summary>
public interface ITrialBalanceReportService
{
    /// <summary>
    /// Generates a Trial Balance report as of the specified date.
    /// </summary>
    /// <param name="asOfDate">The date to generate trial balance for.</param>
    /// <param name="periodId">Optional accounting period filter.</param>
    /// <returns>PDF file as byte array.</returns>
    Task<byte[]> GenerateReportAsync(DateTime asOfDate, Guid? periodId = null);
}
