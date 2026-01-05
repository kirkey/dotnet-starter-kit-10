using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.AccountsPayable.UpdateAccountsPayableAccount;

public record UpdateAccountsPayableAccountCommand(Guid Id, string Name, string? Description) : ICommand<Guid>;