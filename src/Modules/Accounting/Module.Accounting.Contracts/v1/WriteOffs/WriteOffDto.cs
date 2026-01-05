namespace FSH.Module.Accounting.Contracts.v1.WriteOffs;

public record WriteOffDto(
    Guid Id,
    string Name,
    string? Description,
    bool IsActive,
    DateTimeOffset CreatedOnUtc);

public record WriteOffSummaryDto(
    Guid Id,
    string Name,
    bool IsActive);