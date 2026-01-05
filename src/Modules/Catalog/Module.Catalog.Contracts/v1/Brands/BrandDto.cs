namespace FSH.Module.Catalog.Contracts.v1.Brands;

public record BrandDto(
    Guid Id,
    string Name,
    string? Description,
    string? Website,
    bool IsActive,
    DateTimeOffset CreatedOnUtc,
    string? CreatedByUserName);
