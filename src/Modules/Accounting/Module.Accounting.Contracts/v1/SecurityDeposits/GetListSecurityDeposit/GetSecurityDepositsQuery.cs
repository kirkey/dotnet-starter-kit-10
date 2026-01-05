using Mediator;
using FSH.Module.Accounting.Contracts.v1.SecurityDeposits;

namespace FSH.Module.Accounting.Contracts.v1.SecurityDeposits.GetListSecurityDeposit;

public sealed record GetSecurityDepositsQuery(
    int Page = 1,
    int PageSize = 10,
    string? SearchTerm = null,
    bool? IsActive = null) : IQuery<SecurityDepositsPagedResponse>;

public sealed record SecurityDepositsPagedResponse(
    List<SecurityDepositSummaryDto> Items,
    int TotalCount,
    int Page,
    int PageSize);
