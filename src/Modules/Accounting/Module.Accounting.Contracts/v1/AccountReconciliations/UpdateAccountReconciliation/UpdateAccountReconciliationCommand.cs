using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.AccountReconciliations.UpdateAccountReconciliation;

public record UpdateAccountReconciliationCommand(Guid Id, string Name, string? Description) : ICommand<Guid>;