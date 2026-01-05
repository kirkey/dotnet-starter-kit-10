using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.Branches.DeleteBranch;

public sealed record DeleteBranchCommand(Guid Id) : ICommand;
