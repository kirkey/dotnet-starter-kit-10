using FSH.Framework.Core.Paging;
using Mediator;

namespace FSH.Module.Catalog.Contracts.v1.Categories;

/// <summary>
/// Query to get a paginated list of categories.
/// 
/// **Purpose:**
/// Retrieves a paginated list of categories with optional filtering.
/// 
/// **Response:**
/// Returns a PagedList of CategoryDto items.
/// </summary>
public record GetCategoriesQuery : IQuery<PagedList<CategoryDto>>
{
    /// <summary>Gets the search term for filtering categories (optional).</summary>
    public string? Search { get; init; }
    
    /// <summary>Gets the parent category ID filter (optional).</summary>
    public Guid? ParentCategoryId { get; init; }
    
    /// <summary>Gets the page number (default: 1).</summary>
    public int PageNumber { get; init; } = 1;
    
    /// <summary>Gets the page size (default: 10).</summary>
    public int PageSize { get; init; } = 10;
    
    /// <summary>Gets the order by field (default: name).</summary>
    public string? OrderBy { get; init; }
}
