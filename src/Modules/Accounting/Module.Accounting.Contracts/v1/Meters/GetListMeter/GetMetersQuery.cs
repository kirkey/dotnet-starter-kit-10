using Mediator;
using FSH.Module.Accounting.Contracts.v1.Meters;

namespace FSH.Module.Accounting.Contracts.v1.Meters.GetListMeter;

public sealed record GetMetersQuery(
    int Page = 1,
    int PageSize = 10,
    string? SearchTerm = null,
    bool? IsActive = null) : IQuery<MetersPagedResponse>;

public sealed record MetersPagedResponse(
    List<MeterSummaryDto> Items,
    int TotalCount,
    int Page,
    int PageSize);
