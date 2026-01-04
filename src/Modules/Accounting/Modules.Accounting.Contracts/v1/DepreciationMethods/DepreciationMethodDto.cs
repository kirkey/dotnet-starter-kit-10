namespace FSH.Modules.Accounting.Contracts.v1.DepreciationMethods;

public record DepreciationMethodDto(
    Guid Id,
    string Name,
    string? Description,
    bool IsActive,
    DateTimeOffset CreatedOnUtc);

public record DepreciationMethodSummaryDto(
    Guid Id,
    string Name,
    bool IsActive);
