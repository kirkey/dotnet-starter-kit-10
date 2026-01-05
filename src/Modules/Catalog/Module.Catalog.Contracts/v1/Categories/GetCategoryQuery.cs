using Mediator;

namespace FSH.Module.Catalog.Contracts.v1.Categories;

/// <summary>
/// Query to get a single category by ID.
/// 
/// **Purpose:**
/// Retrieves details of a specific category.
/// 
/// **Response:**
/// Returns a CategoryDto with the category details.
/// </summary>
public record GetCategoryQuery : IQuery<CategoryDto>
{
    /// <summary>Gets the ID of the category to retrieve (required).</summary>
    public required Guid Id { get; init; }
}
