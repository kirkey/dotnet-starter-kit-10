namespace FSH.Module.Accounting.Contracts.v1.Projects;

public record ProjectDto(
    Guid Id,
    string Name,
    string? Description,
    bool IsActive,
    DateTimeOffset CreatedOnUtc);

public record ProjectSummaryDto(
    Guid Id,
    string Name,
    bool IsActive);