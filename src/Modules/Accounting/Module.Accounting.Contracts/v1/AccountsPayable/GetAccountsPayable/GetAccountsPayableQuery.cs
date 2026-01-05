using FSH.Module.Accounting.Contracts.v1.AccountsPayable;
using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.AccountsPayable.GetAccountsPayable;

public record GetAccountsPayableQuery(
    int Page = 1,
    int PageSize = 10,
    string? SearchTerm = null,
    bool? IsActive = null) : IQuery<AccountsPayablePagedResponse>;

public record AccountsPayablePagedResponse(
    List<AccountsPayableAccountSummaryDto> Items,
    int TotalCount,
    int Page,
    int PageSize);