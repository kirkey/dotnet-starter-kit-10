using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.AccountsReceivable.DeleteAccountsReceivableAccount;

public record DeleteAccountsReceivableAccountCommand(Guid Id) : ICommand;