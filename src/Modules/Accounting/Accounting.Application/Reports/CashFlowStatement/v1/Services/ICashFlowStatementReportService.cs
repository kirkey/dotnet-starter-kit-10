namespace Accounting.Application.Reports.CashFlowStatement.v1.Services;

/// <summary>
/// Service interface for generating Cash Flow Statement PDF reports using QuestPDF.
/// </summary>
public interface ICashFlowStatementReportService
{
    /// <summary>
    /// Generates a Cash Flow Statement PDF report for the specified period.
    /// </summary>
    /// <param name="startDate">Start date of the reporting period.</param>
    /// <param name="endDate">End date of the reporting period.</param>
    /// <param name="includeComparative">Whether to include comparative period data.</param>
    /// <param name="comparativeStartDate">Optional comparative period start date.</param>
    /// <param name="comparativeEndDate">Optional comparative period end date.</param>
    /// <returns>PDF file as byte array.</returns>
    Task<byte[]> GenerateReportAsync(
        DateTime startDate,
        DateTime endDate,
        bool includeComparative = false,
        DateTime? comparativeStartDate = null,
        DateTime? comparativeEndDate = null);
}
