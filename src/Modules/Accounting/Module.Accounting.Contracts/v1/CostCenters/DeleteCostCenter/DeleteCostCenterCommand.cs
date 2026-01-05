using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.CostCenters.DeleteCostCenter;

public sealed record DeleteCostCenterCommand(Guid Id) : ICommand;