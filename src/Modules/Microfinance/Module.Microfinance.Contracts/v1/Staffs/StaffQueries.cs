namespace FSH.Module.Microfinance.Contracts.v1.Staffs;

public record GetStaffQuery(Guid Id);
public record GetStaffsQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive);
public record StaffsPagedResponse(List<StaffSummaryDto> Items, int TotalCount, int Page, int PageSize);
