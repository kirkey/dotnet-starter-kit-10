namespace Accounting.Application.CostCenters.Create.v1;

/// <summary>
/// Command to create a new cost center.
/// </summary>
public record CostCenterCreateCommand(
    string Code,
    string Name,
    string CostCenterType,
    DefaultIdType? ParentCostCenterId,
    DefaultIdType? ManagerId,
    string? ManagerName,
    decimal BudgetAmount,
    string? Description,
    string? Notes
) : IRequest<CostCenterCreateResponse>;

