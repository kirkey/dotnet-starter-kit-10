namespace Accounting.Application.Reports.BalanceSheet.v1.Services;

/// <summary>
/// Service interface for generating Balance Sheet PDF reports using QuestPDF.
/// </summary>
public interface IBalanceSheetReportService
{
    /// <summary>
    /// Generates a Balance Sheet PDF report as of the specified date.
    /// </summary>
    /// <param name="asOfDate">The date for which to generate the balance sheet.</param>
    /// <param name="includeComparative">Whether to include comparative period data.</param>
    /// <param name="comparativeAsOfDate">Optional comparative period date.</param>
    /// <returns>PDF file as byte array.</returns>
    Task<byte[]> GenerateReportAsync(
        DateTime asOfDate, 
        bool includeComparative = false, 
        DateTime? comparativeAsOfDate = null);
}
