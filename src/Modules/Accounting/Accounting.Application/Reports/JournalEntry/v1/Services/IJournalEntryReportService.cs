namespace Accounting.Application.Reports.JournalEntry.v1.Services;

/// <summary>
/// Service interface for generating Journal Entry PDF reports using QuestPDF.
/// </summary>
public interface IJournalEntryReportService
{
    /// <summary>
    /// Generates a Journal Entry report for the specified date range.
    /// </summary>
    /// <param name="startDate">Start date of the report period.</param>
    /// <param name="endDate">End date of the report period.</param>
    /// <param name="isPostedOnly">Filter to show only posted entries.</param>
    /// <returns>PDF file as byte array.</returns>
    Task<byte[]> GenerateReportAsync(DateTime startDate, DateTime endDate, bool isPostedOnly = false);
}
