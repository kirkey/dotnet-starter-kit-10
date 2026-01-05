using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.BranchTargets.UpdateBranchTarget;

public sealed record UpdateBranchTargetCommand(Guid Id, string Name) : ICommand<Guid>;
