namespace FSH.Module.Accounting.Contracts.v1.FixedAssets;

public record FixedAssetDto(
    Guid Id,
    string Name,
    string? Description,
    DateTime? AcquisitionDate,
    decimal Cost,
    decimal ResidualValue,
    decimal DepreciationRate,
    decimal AccumulatedDepreciation,
    bool IsDisposed,
    DateTime? DisposalDate,
    decimal? DisposalProceeds,
    bool IsActive,
    DateTimeOffset CreatedOnUtc);

public record FixedAssetSummaryDto(
    Guid Id,
    string Name,
    decimal Cost,
    decimal AccumulatedDepreciation,
    bool IsDisposed,
    bool IsActive);
