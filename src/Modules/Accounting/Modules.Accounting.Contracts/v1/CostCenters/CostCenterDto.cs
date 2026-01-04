namespace FSH.Modules.Accounting.Contracts.v1.CostCenters;

public record CostCenterDto(
    Guid Id,
    string Name,
    string? Description,
    bool IsActive,
    DateTimeOffset CreatedOnUtc);

public record CostCenterSummaryDto(
    Guid Id,
    string Name,
    bool IsActive);
