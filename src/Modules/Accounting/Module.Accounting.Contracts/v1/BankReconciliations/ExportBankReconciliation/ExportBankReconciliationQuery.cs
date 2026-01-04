using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.BankReconciliations.ExportBankReconciliation;


/// <summary>
/// Query to export a Bank Reconciliation report for a specific reconciliation instance.
/// </summary>
/// <param name="Id">BankReconciliation Id to export</param>
public record ExportBankReconciliationQuery(Guid Id, string Format = "pdf") : IQuery<ExportBankReconciliationResult>;