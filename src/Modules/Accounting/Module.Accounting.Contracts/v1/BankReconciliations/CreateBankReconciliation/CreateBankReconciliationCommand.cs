using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.BankReconciliations.CreateBankReconciliation;

public record CreateBankReconciliationCommand(
    string ReconciliationNumber,
    Guid BankAccountId,
    DateTime StatementDate,
    decimal StatementBalance,
    decimal BookBalance,
    string? Description = null) : ICommand<Guid>;