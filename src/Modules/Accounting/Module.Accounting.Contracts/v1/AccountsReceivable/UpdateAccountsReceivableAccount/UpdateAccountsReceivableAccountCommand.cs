using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.AccountsReceivable.UpdateAccountsReceivableAccount;

public record UpdateAccountsReceivableAccountCommand(Guid Id, string Name, string? Description) : ICommand<Guid>;