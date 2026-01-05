using Mediator;

namespace FSH.Module.Catalog.Contracts.v1.Categories;

/// <summary>
/// Command to create a new category.
/// 
/// **Purpose:**
/// Initiates the creation of a new product category.
/// 
/// **Properties:**
/// - Name: The name of the category (required)
/// - Code: Unique code for the category (required)
/// - Description: Optional detailed description
/// - ParentId: Optional parent category ID for hierarchical structure
/// 
/// **Response:**
/// Returns the GUID of the newly created category.
/// </summary>
public record CreateCategoryCommand : ICommand<Guid>
{
    /// <summary>
    /// Gets the name of the category to create (required).
    /// </summary>
    public required string Name { get; init; }
    
    /// <summary>
    /// Gets the unique code for the category (required).
    /// </summary>
    public required string Code { get; init; }
    
    /// <summary>
    /// Gets the description of the category (optional).
    /// </summary>
    public string? Description { get; init; }
    
    /// <summary>
    /// Gets the parent category ID for hierarchical organization (optional).
    /// </summary>
    public Guid? ParentId { get; init; }
}
