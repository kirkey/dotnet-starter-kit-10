using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.CostCenters.GetCostCenter;

public sealed record GetCostCenterQuery(Guid Id) : IQuery<CostCenterDto>;
