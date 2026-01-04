using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.AccountingPeriods.GetListAccountingPeriod;


public record GetAccountingPeriodsQuery(
    int Page = 1,
    int PageSize = 10,
    string? SearchTerm = null,
    bool? IsActive = null) : IQuery<AccountingPeriodsPagedResponse>;

public record AccountingPeriodsPagedResponse(
    List<AccountingPeriodSummaryDto> Items,
    int TotalCount,
    int Page,
    int PageSize);