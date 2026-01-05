namespace FSH.Module.Catalog.Contracts.v1.Categories;

/// <summary>
/// Data transfer object for Category entity.
/// </summary>
public record CategoryDto(
    Guid Id,
    string Name,
    string Code,
    string? Description,
    Guid? ParentCategoryId,
    string? ParentCategoryName,
    bool IsActive,
    DateTimeOffset CreatedOnUtc,
    string? CreatedByUserName);
