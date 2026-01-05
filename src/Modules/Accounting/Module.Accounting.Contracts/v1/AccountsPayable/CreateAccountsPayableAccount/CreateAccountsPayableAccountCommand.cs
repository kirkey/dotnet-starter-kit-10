using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.AccountsPayable.CreateAccountsPayableAccount;

public record CreateAccountsPayableAccountCommand(string Name, string? Description) : ICommand<Guid>;