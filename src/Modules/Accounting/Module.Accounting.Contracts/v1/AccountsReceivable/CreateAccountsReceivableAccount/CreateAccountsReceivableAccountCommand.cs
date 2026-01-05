using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.AccountsReceivable.CreateAccountsReceivableAccount;

public record CreateAccountsReceivableAccountCommand(string Name, string? Description) : ICommand<Guid>;