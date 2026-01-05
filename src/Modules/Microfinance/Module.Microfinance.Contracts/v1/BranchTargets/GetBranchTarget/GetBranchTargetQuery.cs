using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.BranchTargets.GetBranchTarget;

public sealed record GetBranchTargetQuery(Guid Id) : IQuery<BranchTargetDto>;
