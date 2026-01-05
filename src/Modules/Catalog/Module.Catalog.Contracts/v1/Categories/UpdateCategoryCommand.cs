using Mediator;

namespace FSH.Module.Catalog.Contracts.v1.Categories;

/// <summary>
/// Command to update an existing category.
/// 
/// **Purpose:**
/// Updates an existing category's details.
/// 
/// **Properties:**
/// - Id: The ID of the category to update (required)
/// - Name: The new name of the category (required)
/// - Code: The new unique code for the category (required)
/// - Description: Optional new description
/// - ParentId: Optional new parent category ID
/// 
/// **Response:**
/// Returns Unit.Value upon successful update.
/// </summary>
public record UpdateCategoryCommand : ICommand
{
    /// <summary>
    /// Gets the ID of the category to update (required).
    /// </summary>
    public required Guid Id { get; init; }
    
    /// <summary>
    /// Gets the new name of the category (required).
    /// </summary>
    public required string Name { get; init; }
    
    /// <summary>
    /// Gets the new unique code for the category (required).
    /// </summary>
    public required string Code { get; init; }
    
    /// <summary>
    /// Gets the new description of the category (optional).
    /// </summary>
    public string? Description { get; init; }
    
    /// <summary>
    /// Gets the new parent category ID (optional).
    /// </summary>
    public Guid? ParentId { get; init; }
}
