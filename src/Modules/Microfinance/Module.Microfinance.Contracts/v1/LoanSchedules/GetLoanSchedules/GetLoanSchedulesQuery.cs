using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.LoanSchedules.GetLoanSchedules;

public sealed record GetLoanSchedulesQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive) : IQuery<LoanSchedulesPagedResponse>;

public sealed record LoanSchedulesPagedResponse(List<LoanScheduleSummaryDto> Items, int TotalCount, int Page, int PageSize);
