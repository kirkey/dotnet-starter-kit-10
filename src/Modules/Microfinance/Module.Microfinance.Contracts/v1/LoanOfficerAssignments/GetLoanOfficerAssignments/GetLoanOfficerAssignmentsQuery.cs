using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.LoanOfficerAssignments.GetLoanOfficerAssignments;

public sealed record GetLoanOfficerAssignmentsQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive) : IQuery<LoanOfficerAssignmentsPagedResponse>;

public sealed record LoanOfficerAssignmentsPagedResponse(List<LoanOfficerAssignmentSummaryDto> Items, int TotalCount, int Page, int PageSize);
