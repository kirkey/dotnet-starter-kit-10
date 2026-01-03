namespace FSH.Modules.Microfinance.Contracts.v1.ApprovalWorkflows;

public record GetApprovalWorkflowQuery(Guid Id);
public record GetApprovalWorkflowsQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive);
public record ApprovalWorkflowsPagedResponse(List<ApprovalWorkflowSummaryDto> Items, int TotalCount, int Page, int PageSize);
