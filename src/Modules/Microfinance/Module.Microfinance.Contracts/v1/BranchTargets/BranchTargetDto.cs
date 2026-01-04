namespace FSH.Module.Microfinance.Contracts.v1.BranchTargets;

public record BranchTargetDto(
    Guid Id,
    string Name,
    bool IsActive,
    DateTimeOffset CreatedOnUtc);

public record BranchTargetSummaryDto(
    Guid Id,
    string Name,
    bool IsActive);
