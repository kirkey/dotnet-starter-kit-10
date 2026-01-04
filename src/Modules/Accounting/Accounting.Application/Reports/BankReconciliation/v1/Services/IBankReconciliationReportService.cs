namespace Accounting.Application.Reports.BankReconciliation.v1.Services;

/// <summary>
/// Service interface for generating Bank Reconciliation PDF reports using QuestPDF.
/// </summary>
public interface IBankReconciliationReportService
{
    /// <summary>
    /// Generates a Bank Reconciliation PDF report.
    /// </summary>
    /// <param name="reconciliationId">The reconciliation ID to generate the report for.</param>
    /// <returns>PDF file as byte array.</returns>
    Task<byte[]> GenerateReportAsync(DefaultIdType reconciliationId);
}
