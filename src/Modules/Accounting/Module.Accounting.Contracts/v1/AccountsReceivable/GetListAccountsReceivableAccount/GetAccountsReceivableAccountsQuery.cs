using FSH.Module.Accounting.Contracts.v1.AccountsReceivable;
using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.AccountsReceivable.GetListAccountsReceivableAccount;

public record GetAccountsReceivableAccountsQuery(
    int Page = 1,
    int PageSize = 10,
    string? SearchTerm = null,
    bool? IsActive = null) : IQuery<AccountsReceivableAccountsPagedResponse>;

public record AccountsReceivableAccountsPagedResponse(
    List<AccountsReceivableAccountSummaryDto> Items,
    int TotalCount,
    int Page,
    int PageSize);