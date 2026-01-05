using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.ApprovalRequests.GetApprovalRequest;

public sealed record GetApprovalRequestQuery(Guid Id) : IQuery<ApprovalRequestDto>;
