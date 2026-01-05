using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.ProjectCosts.UpdateProjectCost;

public record UpdateProjectCostCommand(Guid Id, string Name, string? Description) : ICommand<Guid>;