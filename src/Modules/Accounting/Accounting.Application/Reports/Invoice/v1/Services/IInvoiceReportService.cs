namespace Accounting.Application.Reports.Invoice.v1.Services;

/// <summary>
/// Service interface for generating individual Invoice PDF reports using QuestPDF.
/// </summary>
public interface IInvoiceReportService
{
    /// <summary>
    /// Generates an individual Invoice PDF report.
    /// </summary>
    /// <param name="invoiceId">The invoice ID to generate the report for.</param>
    /// <returns>PDF file as byte array.</returns>
    Task<byte[]> GenerateReportAsync(DefaultIdType invoiceId);
}
