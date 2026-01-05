using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.ApprovalRequests.DeleteApprovalRequest;

public sealed record DeleteApprovalRequestCommand(Guid Id) : ICommand;
