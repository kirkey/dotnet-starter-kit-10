using Mediator;

namespace FSH.Module.Catalog.Contracts.v1.Categories;

/// <summary>
/// Query to get a list of categories with optional filtering.
/// 
/// **Purpose:**
/// Retrieves a list of categories with support for search and filtering.
/// 
/// **Properties:**
/// - Search: Optional search term to filter by name or code
/// - ParentId: Optional parent category ID to filter by hierarchy
/// - IncludeInactive: Whether to include inactive categories (default: false)
/// 
/// **Response:**
/// Returns a list of CategoryResponse objects.
/// </summary>
public record GetCategoriesQuery : IQuery<List<CategoryResponse>>
{
    /// <summary>
    /// Gets the optional search term to filter categories.
    /// </summary>
    public string? Search { get; init; }
    
    /// <summary>
    /// Gets the optional parent category ID to filter by hierarchy.
    /// </summary>
    public Guid? ParentId { get; init; }
    
    /// <summary>
    /// Gets whether to include inactive categories in the results.
    /// </summary>
    public bool IncludeInactive { get; init; }
}
