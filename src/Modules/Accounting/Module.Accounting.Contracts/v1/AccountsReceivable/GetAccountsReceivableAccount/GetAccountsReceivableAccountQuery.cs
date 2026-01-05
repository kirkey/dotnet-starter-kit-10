using FSH.Module.Accounting.Contracts.v1.AccountsReceivable;
using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.AccountsReceivable.GetAccountsReceivableAccount;

public record GetAccountsReceivableAccountQuery(Guid Id) : IQuery<AccountsReceivableAccountDto>;