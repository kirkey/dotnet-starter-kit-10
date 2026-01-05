using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.CostCenters.CreateCostCenter;

public sealed record CreateCostCenterCommand(string Name, string? Description = null) : ICommand<Guid>;