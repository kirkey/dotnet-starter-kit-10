using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.ProjectCosts.DeleteProjectCost;

public record DeleteProjectCostCommand(Guid Id) : ICommand;