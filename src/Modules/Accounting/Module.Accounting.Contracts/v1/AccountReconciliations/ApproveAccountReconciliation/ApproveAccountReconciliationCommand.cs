using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.AccountReconciliations.ApproveAccountReconciliation;

public sealed record ApproveAccountReconciliationCommand(Guid Id) : ICommand;
