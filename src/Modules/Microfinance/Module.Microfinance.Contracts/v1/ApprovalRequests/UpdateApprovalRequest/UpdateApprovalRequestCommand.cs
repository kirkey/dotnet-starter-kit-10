using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.ApprovalRequests.UpdateApprovalRequest;

public sealed record UpdateApprovalRequestCommand(Guid Id, string Name) : ICommand<Guid>;
