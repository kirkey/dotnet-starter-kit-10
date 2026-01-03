namespace FSH.Modules.Microfinance.Contracts.v1.LoanOfficerAssignments;

public record GetLoanOfficerAssignmentQuery(Guid Id);
public record GetLoanOfficerAssignmentsQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive);
public record LoanOfficerAssignmentsPagedResponse(List<LoanOfficerAssignmentSummaryDto> Items, int TotalCount, int Page, int PageSize);
