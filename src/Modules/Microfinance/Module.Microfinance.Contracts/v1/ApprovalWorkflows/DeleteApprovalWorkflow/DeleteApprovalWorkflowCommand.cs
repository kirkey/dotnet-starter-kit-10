using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.ApprovalWorkflows.DeleteApprovalWorkflow;

public sealed record DeleteApprovalWorkflowCommand(Guid Id) : ICommand;
