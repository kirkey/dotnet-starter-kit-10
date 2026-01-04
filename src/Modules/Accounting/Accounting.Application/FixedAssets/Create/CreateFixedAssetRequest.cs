namespace Accounting.Application.FixedAssets.Create;

/// <summary>
/// Command to create a new Fixed Asset.
/// </summary>
public sealed record CreateFixedAssetCommand(
    string AssetName,
    DateTime PurchaseDate,
    decimal PurchasePrice,
    DefaultIdType DepreciationMethodId,
    int ServiceLife,
    decimal SalvageValue,
    DefaultIdType AccumulatedDepreciationAccountId,
    DefaultIdType DepreciationExpenseAccountId,
    string AssetType,
    string? SerialNumber,
    string? Location,
    string? Department,
    string? GpsCoordinates,
    string? SubstationName,
    DefaultIdType? AssetUsoaId,
    string? RegulatoryClassification,
    decimal? VoltageRating,
    decimal? Capacity,
    string? Manufacturer,
    string? ModelNumber,
    bool RequiresUsoaReporting,
    string? Description,
    string? Notes,
    string? ImageUrl
) : IRequest<CreateFixedAssetResponse>;
