using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.BankReconciliations.ApproveBankReconciliation;


/// <summary>
/// Command to approve and finalize a bank reconciliation, applying reconciled items.
/// </summary>
/// <param name="Id">BankReconciliation ID to approve</param>
public record ApproveBankReconciliationCommand(Guid Id) : ICommand;