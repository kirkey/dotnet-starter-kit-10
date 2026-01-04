namespace FSH.Module.Microfinance.Contracts.v1.LoanSchedules;

public record GetLoanScheduleQuery(Guid Id);
public record GetLoanSchedulesQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive);
public record LoanSchedulesPagedResponse(List<LoanScheduleSummaryDto> Items, int TotalCount, int Page, int PageSize);
