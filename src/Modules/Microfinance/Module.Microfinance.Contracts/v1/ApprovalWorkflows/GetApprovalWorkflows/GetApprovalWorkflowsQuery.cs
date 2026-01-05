using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.ApprovalWorkflows.GetApprovalWorkflows;

public sealed record GetApprovalWorkflowsQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive) : IQuery<ApprovalWorkflowsPagedResponse>;
