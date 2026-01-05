using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.BankReconciliations.CreateBankReconciliation;

public record CreateBankReconciliationCommand(string Name, string? Description) : ICommand<Guid>;