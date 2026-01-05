using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.ApprovalRequests.CreateApprovalRequest;

public sealed record CreateApprovalRequestCommand(string Name) : ICommand<Guid>;
