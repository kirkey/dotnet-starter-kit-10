using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.Staffs.GetStaffs;

public sealed record GetStaffsQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive) : IQuery<StaffsPagedResponse>;

public sealed record StaffsPagedResponse(List<StaffSummaryDto> Items, int TotalCount, int Page, int PageSize);
