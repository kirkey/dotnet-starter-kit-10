using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.ApprovalWorkflows.CreateApprovalWorkflow;

public sealed record CreateApprovalWorkflowCommand(string Name) : ICommand<Guid>;
