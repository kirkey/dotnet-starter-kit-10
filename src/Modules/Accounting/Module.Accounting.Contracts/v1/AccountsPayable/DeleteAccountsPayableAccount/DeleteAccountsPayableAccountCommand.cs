using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.AccountsPayable.DeleteAccountsPayableAccount;

public record DeleteAccountsPayableAccountCommand(Guid Id) : ICommand;