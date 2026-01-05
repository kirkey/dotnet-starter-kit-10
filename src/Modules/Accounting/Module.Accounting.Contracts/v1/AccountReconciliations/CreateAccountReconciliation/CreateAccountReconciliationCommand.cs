using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.AccountReconciliations.CreateAccountReconciliation;

public record CreateAccountReconciliationCommand(string Name, string? Description) : ICommand<Guid>;