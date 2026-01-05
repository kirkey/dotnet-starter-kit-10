using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.Branches.GetBranch;

public sealed record GetBranchQuery(Guid Id) : IQuery<BranchDto>;
