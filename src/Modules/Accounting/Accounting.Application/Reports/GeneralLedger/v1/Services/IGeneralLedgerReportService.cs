namespace Accounting.Application.Reports.GeneralLedger.v1.Services;

/// <summary>
/// Service interface for generating General Ledger PDF reports using QuestPDF.
/// </summary>
public interface IGeneralLedgerReportService
{
    /// <summary>
    /// Generates a General Ledger report for the specified date range.
    /// </summary>
    /// <param name="startDate">Start date of the report period.</param>
    /// <param name="endDate">End date of the report period.</param>
    /// <param name="accountId">Optional filter for a specific account.</param>
    /// <returns>PDF file as byte array.</returns>
    Task<byte[]> GenerateReportAsync(DateTime startDate, DateTime endDate, Guid? accountId = null);
}
