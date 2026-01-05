using FSH.Module.Accounting.Contracts.v1.AccountsPayable;
using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.AccountsPayable.GetAccountsPayableAccount;

public record GetAccountsPayableAccountQuery(Guid Id) : IQuery<AccountsPayableAccountDto>;