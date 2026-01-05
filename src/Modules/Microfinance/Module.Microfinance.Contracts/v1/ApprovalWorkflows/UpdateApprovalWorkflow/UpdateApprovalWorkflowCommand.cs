using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.ApprovalWorkflows.UpdateApprovalWorkflow;

public sealed record UpdateApprovalWorkflowCommand(Guid Id, string Name) : ICommand<Guid>;
