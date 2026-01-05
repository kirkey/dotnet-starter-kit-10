using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.Branches.CreateBranch;

public sealed record CreateBranchCommand(string Name) : ICommand<Guid>;
