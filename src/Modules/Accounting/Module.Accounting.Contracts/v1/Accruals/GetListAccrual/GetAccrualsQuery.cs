using FSH.Module.Accounting.Contracts.v1.Accruals;
using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.Accruals.GetListAccrual;

public record GetAccrualsQuery(
    int Page = 1,
    int PageSize = 10,
    string? SearchTerm = null,
    bool? IsActive = null) : IQuery<AccrualsPagedResponse>;

public record AccrualsPagedResponse(
    List<AccrualSummaryDto> Items,
    int TotalCount,
    int Page,
    int PageSize);