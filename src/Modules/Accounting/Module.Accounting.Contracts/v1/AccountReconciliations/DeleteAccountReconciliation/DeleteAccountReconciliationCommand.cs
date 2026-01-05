using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.AccountReconciliations.DeleteAccountReconciliation;

public record DeleteAccountReconciliationCommand(Guid Id) : ICommand;