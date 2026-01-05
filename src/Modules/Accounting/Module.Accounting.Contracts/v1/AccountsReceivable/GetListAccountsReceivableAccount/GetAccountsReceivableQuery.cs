using Mediator;
using FSH.Module.Accounting.Contracts.v1.AccountsReceivable;

namespace FSH.Module.Accounting.Contracts.v1.AccountsReceivable.GetListAccountsReceivableAccount;

public sealed record GetAccountsReceivableQuery(
    int Page = 1,
    int PageSize = 10,
    string? SearchTerm = null,
    bool? IsActive = null) : IQuery<AccountsReceivablePagedResponse>;

public sealed record AccountsReceivablePagedResponse(
    List<AccountsReceivableAccountSummaryDto> Items,
    int TotalCount,
    int Page,
    int PageSize);
