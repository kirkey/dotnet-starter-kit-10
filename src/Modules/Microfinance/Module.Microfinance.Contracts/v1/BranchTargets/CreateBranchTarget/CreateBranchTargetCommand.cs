using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.BranchTargets.CreateBranchTarget;

public sealed record CreateBranchTargetCommand(string Name) : ICommand<Guid>;
