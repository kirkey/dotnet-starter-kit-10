using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.ApprovalWorkflows.GetApprovalWorkflow;

public sealed record GetApprovalWorkflowQuery(Guid Id) : IQuery<ApprovalWorkflowDto>;
