using FSH.Module.Accounting.Contracts.v1.AccountsPayable;
using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.AccountsPayable.GetListAccountsPayableAccount;

public record GetAccountsPayableAccountsQuery(
    int Page = 1,
    int PageSize = 10,
    string? SearchTerm = null,
    bool? IsActive = null) : IQuery<AccountsPayableAccountsPagedResponse>;

public record AccountsPayableAccountsPagedResponse(
    List<AccountsPayableAccountSummaryDto> Items,
    int TotalCount,
    int Page,
    int PageSize);