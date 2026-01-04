namespace Accounting.Application.Reports.CustomerStatement.v1.Services;

/// <summary>
/// Service interface for generating Customer Statement PDF reports using QuestPDF.
/// </summary>
public interface ICustomerStatementReportService
{
    /// <summary>
    /// Generates a Customer Statement PDF report showing account activity.
    /// </summary>
    /// <param name="customerId">The customer ID to generate the statement for.</param>
    /// <param name="startDate">Start date of the statement period.</param>
    /// <param name="endDate">End date of the statement period.</param>
    /// <returns>PDF file as byte array.</returns>
    Task<byte[]> GenerateReportAsync(DefaultIdType customerId, DateTime startDate, DateTime endDate);
}
