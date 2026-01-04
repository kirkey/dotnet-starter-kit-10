namespace FSH.Module.Accounting.Contracts.v1.ProjectCosts;

public record ProjectCostDto(
    Guid Id,
    string Name,
    string? Description,
    bool IsActive,
    DateTimeOffset CreatedOnUtc);

public record ProjectCostSummaryDto(
    Guid Id,
    string Name,
    bool IsActive);
