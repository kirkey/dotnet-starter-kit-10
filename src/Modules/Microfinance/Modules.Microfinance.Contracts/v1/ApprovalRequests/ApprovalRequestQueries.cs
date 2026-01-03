namespace FSH.Modules.Microfinance.Contracts.v1.ApprovalRequests;

public record GetApprovalRequestQuery(Guid Id);
public record GetApprovalRequestsQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive);
public record ApprovalRequestsPagedResponse(List<ApprovalRequestSummaryDto> Items, int TotalCount, int Page, int PageSize);
