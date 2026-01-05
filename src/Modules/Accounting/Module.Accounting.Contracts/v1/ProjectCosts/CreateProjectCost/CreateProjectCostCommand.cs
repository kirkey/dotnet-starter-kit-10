using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.ProjectCosts.CreateProjectCost;

public record CreateProjectCostCommand(string Name, string? Description) : ICommand<Guid>;