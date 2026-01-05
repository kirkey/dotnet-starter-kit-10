using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.ApprovalRequests.GetApprovalRequests;

public sealed record GetApprovalRequestsQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive) : IQuery<ApprovalRequestsPagedResponse>;
