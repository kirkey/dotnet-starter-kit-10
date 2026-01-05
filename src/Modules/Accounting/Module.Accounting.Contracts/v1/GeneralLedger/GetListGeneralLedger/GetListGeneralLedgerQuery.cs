using FSH.Module.Accounting.Contracts.v1.GeneralLedger;
using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.GeneralLedger.GetListGeneralLedger;

public record GetListGeneralLedgerQuery(
    int Page = 1,
    int PageSize = 10,
    string? SearchTerm = null,
    bool? IsActive = null) : IQuery<GeneralLedgerPagedResponse>;

public record GeneralLedgerPagedResponse(
    List<GeneralLedgerSummaryDto> Items,
    int TotalCount,
    int Page,
    int PageSize);
