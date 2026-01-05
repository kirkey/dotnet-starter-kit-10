using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.BranchTargets.DeleteBranchTarget;

public sealed record DeleteBranchTargetCommand(Guid Id) : ICommand;
