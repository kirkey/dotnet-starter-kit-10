using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.ProjectCosts.GetProjectCost;

public record GetProjectCostQuery(Guid Id) : IQuery<ProjectCostDto>;