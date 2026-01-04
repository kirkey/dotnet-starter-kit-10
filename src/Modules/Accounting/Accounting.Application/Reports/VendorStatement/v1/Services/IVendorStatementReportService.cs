namespace Accounting.Application.Reports.VendorStatement.v1.Services;

/// <summary>
/// Service interface for generating Vendor Statement PDF reports using QuestPDF.
/// </summary>
public interface IVendorStatementReportService
{
    /// <summary>
    /// Generates a Vendor Statement PDF report showing account activity.
    /// </summary>
    /// <param name="vendorId">The vendor ID to generate the statement for.</param>
    /// <param name="startDate">Start date of the statement period.</param>
    /// <param name="endDate">End date of the statement period.</param>
    /// <returns>PDF file as byte array.</returns>
    Task<byte[]> GenerateReportAsync(DefaultIdType vendorId, DateTime startDate, DateTime endDate);
}
