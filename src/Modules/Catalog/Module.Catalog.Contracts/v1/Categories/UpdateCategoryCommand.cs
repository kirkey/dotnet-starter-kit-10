using Mediator;

namespace FSH.Module.Catalog.Contracts.v1.Categories;

/// <summary>
/// Command to update an existing category.
/// 
/// **Purpose:**
/// Updates the details of an existing category.
/// 
/// **Response:**
/// Returns Unit on successful update.
/// </summary>
public record UpdateCategoryCommand : ICommand<Unit>
{
    /// <summary>Gets the ID of the category to update (required).</summary>
    public required Guid Id { get; init; }
    
    /// <summary>Gets the updated name of the category (required).</summary>
    public required string Name { get; init; }
    
    /// <summary>Gets the updated unique code for the category (required).</summary>
    public required string Code { get; init; }
    
    /// <summary>Gets the updated description of the category (optional).</summary>
    public string? Description { get; init; }
    
    /// <summary>Gets the updated parent category ID (optional).</summary>
    public Guid? ParentCategoryId { get; init; }
}
