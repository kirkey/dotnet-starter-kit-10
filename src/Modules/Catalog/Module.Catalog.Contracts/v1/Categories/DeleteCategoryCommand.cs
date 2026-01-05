using Mediator;

namespace FSH.Module.Catalog.Contracts.v1.Categories;

/// <summary>
/// Command to delete a category.
/// 
/// **Purpose:**
/// Deletes an existing category from the catalog.
/// 
/// **Properties:**
/// - Id: The ID of the category to delete (required)
/// 
/// **Response:**
/// Returns Unit.Value upon successful deletion.
/// </summary>
public record DeleteCategoryCommand(Guid Id) : ICommand;
