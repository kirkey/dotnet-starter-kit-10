using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.CostCenters.UpdateCostCenter;

public sealed record UpdateCostCenterCommand(Guid Id, string Name, string? Description = null) : ICommand<Guid>;