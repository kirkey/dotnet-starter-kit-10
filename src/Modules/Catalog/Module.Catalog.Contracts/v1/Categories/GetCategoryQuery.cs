using Mediator;

namespace FSH.Module.Catalog.Contracts.v1.Categories;

/// <summary>
/// Query to get a category by ID.
/// 
/// **Purpose:**
/// Retrieves a single category by its unique identifier.
/// 
/// **Properties:**
/// - Id: The ID of the category to retrieve (required)
/// 
/// **Response:**
/// Returns CategoryResponse or null if not found.
/// </summary>
public record GetCategoryQuery(Guid Id) : IQuery<CategoryResponse?>;

/// <summary>
/// Response model for category details.
/// </summary>
public record CategoryResponse(
    Guid Id,
    string Name,
    string Code,
    string? Description,
    Guid? ParentId,
    string? ParentName,
    bool IsActive,
    DateTimeOffset CreatedOnUtc,
    string? CreatedByUserName);
