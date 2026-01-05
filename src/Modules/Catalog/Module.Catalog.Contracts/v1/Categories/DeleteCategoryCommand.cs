using Mediator;

namespace FSH.Module.Catalog.Contracts.v1.Categories;

/// <summary>
/// Command to delete a category.
/// 
/// **Purpose:**
/// Soft deletes a category by marking it as inactive.
/// 
/// **Response:**
/// Returns Unit on successful deletion.
/// </summary>
public record DeleteCategoryCommand : ICommand<Unit>
{
    /// <summary>Gets the ID of the category to delete (required).</summary>
    public required Guid Id { get; init; }
}
