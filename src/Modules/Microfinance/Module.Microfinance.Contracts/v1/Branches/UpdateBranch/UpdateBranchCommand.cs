using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.Branches.UpdateBranch;

public sealed record UpdateBranchCommand(Guid Id, string Name) : ICommand<Guid>;
