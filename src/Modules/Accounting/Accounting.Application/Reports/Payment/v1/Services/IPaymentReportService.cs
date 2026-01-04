namespace Accounting.Application.Reports.Payment.v1.Services;

/// <summary>
/// Service interface for generating Payment Receipt PDF reports using QuestPDF.
/// </summary>
public interface IPaymentReportService
{
    /// <summary>
    /// Generates a Payment Receipt PDF report.
    /// </summary>
    /// <param name="paymentId">The payment ID to generate the receipt for.</param>
    /// <returns>PDF file as byte array.</returns>
    Task<byte[]> GenerateReportAsync(DefaultIdType paymentId);
}
