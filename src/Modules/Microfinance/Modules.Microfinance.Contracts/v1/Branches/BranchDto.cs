namespace FSH.Modules.Microfinance.Contracts.v1.Branches;

public record BranchDto(
    Guid Id,
    string Name,
    bool IsActive,
    DateTimeOffset CreatedOnUtc);

public record BranchSummaryDto(
    Guid Id,
    string Name,
    bool IsActive);
