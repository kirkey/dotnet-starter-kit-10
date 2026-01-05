using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.RateSchedules.GetListRateSchedule;

public sealed record GetRateSchedulesQuery(int Page = 1, int PageSize = 10, string? SearchTerm = null, bool? IsActive = null) : IQuery<RateSchedulesPagedResponse>;

public sealed record RateSchedulesPagedResponse(List<RateScheduleSummaryDto> Items, int TotalCount, int Page, int PageSize);