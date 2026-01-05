using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.Consumption.GetListConsumption;

public sealed record GetConsumptionQuery(
    int Page = 1,
    int PageSize = 10,
    string? SearchTerm = null,
    bool? IsActive = null) : IQuery<ConsumptionPagedResponse>;

public sealed record ConsumptionPagedResponse(
    List<ConsumptionSummaryDto> Items,
    int TotalCount,
    int Page,
    int PageSize);
